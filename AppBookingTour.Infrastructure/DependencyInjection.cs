using AppBookingTour.Application.Common.Settings;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Application.IServices;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Infrastructure.Data;
using AppBookingTour.Infrastructure.Data.Repositories;
using AppBookingTour.Infrastructure.Database;
using AppBookingTour.Infrastructure.Jobs;
using AppBookingTour.Infrastructure.Services;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;

namespace AppBookingTour.Infrastructure;

/// <summary>
/// Infrastructure layer dependency injection
/// Clean Architecture compliant service registration
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        // Database configuration - Single unified context
        AddDatabase(services, configuration);

        // ASP.NET Core Identity Configuration
        AddIdentity(services);

        // JWT Authentication Configuration
        AddJwtAuthentication(services, configuration);

        // Repository Pattern & Unit of Work (Clean Architecture)
        AddRepositoryPattern(services);

        // Business services implementations
        AddBusinessServices(services);

        // External services
        AddExternalServices(services, configuration);
        
        // Payment services
        AddPaymentServices(services, configuration);

        // ? Background Jobs with Hangfire
        AddHangfire(services, configuration);

        return services;
    }

    private static void AddDatabase(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;
    
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString, builder =>
            {
                builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                //builder.EnableRetryOnFailure(
                //    maxRetryCount: 3,
                //    maxRetryDelay: TimeSpan.FromSeconds(30),
                //    errorNumbersToAdd: null);
            });
        
            // Enable in development for detailed logs
            options.EnableSensitiveDataLogging(false);
            options.EnableDetailedErrors(false);
        });
    }

    private static void AddIdentity(IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole<int>>(options =>
        {
            // Password settings
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
            
            // Lockout settings
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
            
            // User settings
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = true;
            options.SignIn.RequireConfirmedPhoneNumber = false;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();
    }

    private static void AddJwtAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JWT");
        
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]!)),
                ValidateIssuer = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidateAudience = true,
                ValidAudience = jwtSettings["Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true
            };

            // Add JWT events for logging and debugging
            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = context =>
                {
                    // Log successful token validation
                    return Task.CompletedTask;
                },
                OnAuthenticationFailed = context =>
                {
                    // Log authentication failures
                    return Task.CompletedTask;
                }
            };
        });
    }

    private static void AddRepositoryPattern(IServiceCollection services)
    {
        // Register Unit of Work (Clean Architecture pattern)
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Register specific repositories
        //services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITourRepository, TourRepository>();
        services.AddScoped<IDiscountRepository, DiscountRepository>();
        services.AddScoped<IAccommodationRepository, AccomodationRepository>();
        services.AddScoped<IBlogPostRepository, BlogPostRepository>();

        // Register generic repository
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    }

    private static void AddBusinessServices(IServiceCollection services)
    {
        // Authentication & JWT services (implement Application interfaces)
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IAuthService, AuthService>();

        // Current user service for accessing authenticated user from JWT
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        // Content security services
        services.AddScoped<IHtmlSanitizerService, HtmlSanitizerService>();

        // Other business services
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IFileStorageService, AzureBlobStorageService>();
        
        // services.AddScoped<IPaymentGatewayService, VNPayService>();
        // services.AddScoped<INotificationService, NotificationService>();
    }

    private static void AddExternalServices(IServiceCollection services, IConfiguration configuration)
    {
        // Redis configuration
        var redisConfiguration = configuration.GetSection("Redis:Configuration").Value;
        if (!string.IsNullOrEmpty(redisConfiguration))
        {
            services.AddSingleton<IConnectionMultiplexer>(provider =>
                ConnectionMultiplexer.Connect(redisConfiguration));
            
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConfiguration;
                options.InstanceName = "AppBookingTour";
            });
        }

        // Add other external services
        // services.AddHttpClient<IEmailService, EmailService>();
        // services.AddScoped<IFileStorageService, AzureBlobStorageService>();
        // services.AddScoped<IPaymentGatewayService, VNPayService>();
    }
    
    private static void AddPaymentServices(IServiceCollection services, IConfiguration configuration)
    {
        // Configure VNPay settings
        services.Configure<VNPaySettings>(configuration.GetSection("VNPay"));
        
        // Register payment services
        services.AddScoped<IVNPayService, VNPayService>();
        services.AddScoped<IQRCodeService, QRCodeService>();
    }

    private static void AddHangfire(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")!;
        
        // Add Hangfire services
        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true,
                SchemaName = "Hangfire"
            }));
        
        // Add the processing server as IHostedService
        services.AddHangfireServer(options =>
        {
            options.WorkerCount = 5; // S? l??ng worker threads
            options.ServerName = "AppBookingTour-Server";
        });

        // Register background jobs
        services.AddScoped<RefundExpiredBookingsJob>();
    }
}