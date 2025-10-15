using AppBookingTour.Api.DataSeeder;
using AppBookingTour.Api.Middlewares;
using AppBookingTour.Application;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Infrastructure;
using AppBookingTour.Infrastructure.Database;
using AppBookingTour.Share.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Serilog;

#region Serilog Configuration
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true, reloadOnChange: true)
        .Build())
    .Enrich.FromLogContext()
    .CreateLogger();

Log.Information("Starting AppBookingTour API application");
#endregion

var builder = WebApplication.CreateBuilder(args);

// Use Serilog
builder.Host.UseSerilog();

#region Core Services
builder.Services.AddControllers();

# region Application Layer
builder.Services.AddApplication();
#endregion

// Add AutoMapper 
builder.Services.AddAutoMapper(
    cfg => { },
    typeof(AssemblyMarker).Assembly
);
#endregion

#region Infrastructure Layer
builder.Services.AddInfrastructure(builder.Configuration);
#endregion

#region Common Services
// Health Checks
builder.Services.AddHealthChecks();

// Output cache for controller attributes
builder.Services.AddOutputCache();

builder.Services.AddLogging();

// Configuration binding
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

// ✅ CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000",
                "https://localhost:3001",
                "http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

#endregion

#region Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AppBookingTour Clean Architecture API",
        Version = "v1",
        Description = "Clean Architecture API with Repository Pattern, Unit of Work, and ASP.NET Core Identity",
        Contact = new OpenApiContact
        {
            Name = "AppBookingTour Team",
            Email = "support@appbookingtour.com"
        }
    });

    // ✅ Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: Authorization: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
#endregion

var app = builder.Build();

# region Seeding Data

using (var scope = app.Services.CreateScope())
{
    // Seed roles first
    await Seeder.SeedRolesAsync(scope.ServiceProvider);
    
    // Seed cities from JSON file
    var citiesJsonPath = Path.Combine(app.Environment.ContentRootPath, "DataSeeder", "vn_provinces_63.json");
    await Seeder.SeedCitiesFromJsonAsync(scope.ServiceProvider, citiesJsonPath);
}

#endregion

// Add Custom Middlewares
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

# region Middleware Pipeline Configuration
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AppBookingTour API V1");
    c.RoutePrefix = "swagger";
});

// Error Handling + HSTS (after custom exception handling)
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

// Security Headers
app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "DENY";
    context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
    await next();
});

app.UseHttpsRedirection();
app.UseSerilogRequestLogging();
app.UseCors("AllowSpecificOrigins");
app.UseRouting();

// Authentication must come before Authorization
app.UseAuthentication();
app.UseAuthorization();

app.UseOutputCache();

app.MapControllers();
app.MapHealthChecks("/health");

// Redirect root to Swagger
app.MapGet("/", () => Results.Redirect("/swagger"));
#endregion

app.Lifetime.ApplicationStarted.Register(() =>
{
    foreach (var address in app.Urls)
        Console.WriteLine($"The application is running at: {address}");
});

Log.Information("Running web host in {Environment}", app.Environment.EnvironmentName);
await app.RunAsync();
