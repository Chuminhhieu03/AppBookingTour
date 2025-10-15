using AppBookingTour.Application.IRepositories;
using AppBookingTour.Application.IServices;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Domain.IRepositories;
using AppBookingTour.Infrastructure.Data;
using AppBookingTour.Infrastructure.Data.Repositories;
using AppBookingTour.Infrastructure.Database;
using AppBookingTour.Infrastructure.Services;
using AutoMapper;
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
                builder.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
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

        // Register generic repository
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    }

    private static void AddBusinessServices(IServiceCollection services)
    {
        // Authentication & JWT services (implement Application interfaces)
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IAuthService, AuthService>();

        // Add other business services here as needed
        services.AddScoped<IEmailService, EmailService>();
        // services.AddScoped<IFileStorageService, AzureBlobStorageService>();
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
    /// <summary>
    /// Extension methods for service configuration validation
    /// </summary>
    //public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    //{
    //    services.AddHealthChecks()
    //        .AddSqlServer(
    //            configuration.GetConnectionString("DefaultConnection")!,
    //            name: "Database",
    //            tags: new[] { "db", "sql", "sqlserver" })
    //        .AddCheck("self", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy());

    //    // Add Redis health check if configured
    //    var redisConfiguration = configuration.GetSection("Redis:Configuration").Value;
    //    if (!string.IsNullOrEmpty(redisConfiguration))
    //    {
    //        services.AddHealthChecks()
    //            .AddRedis(redisConfiguration, name: "Redis", tags: new[] { "cache", "redis" });
    //    }

    //    return services;
    //}
}