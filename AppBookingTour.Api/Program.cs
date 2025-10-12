using AppBookingTour.Api.DataSeeder;
using AppBookingTour.Application;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Infrastructure;
using AppBookingTour.Infrastructure.Database;
using AppBookingTour.Share.Configurations;
using AutoMapper;
using AppBookingTour.Application.Mapping;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Serilog;
using System;

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

// ✅ Add MediatR for CQRS (point to Application layer)
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblies(typeof(AssemblyMarker).Assembly)
);

builder.Services.AddAutoMapper(cfg => cfg.AddMaps(typeof(MappingProfile).Assembly));

// ✅ Add FluentValidation (if you use it)
builder.Services.AddValidatorsFromAssembly(typeof(AssemblyMarker).Assembly);
#endregion

#region Infrastructure Layer
// ✅ Add Infrastructure (DbContext, Identity, Repositories, External services)
builder.Services.AddInfrastructure(builder.Configuration);

// ✅ Add Identity so UserManager<User> & SignInManager<User> can be resolved
builder.Services.AddIdentity<User, IdentityRole<int>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
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
    await Seeder.SeedRolesAsync(scope.ServiceProvider);
}

#endregion

#region Middleware
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AppBookingTour API V1");
    c.RoutePrefix = "swagger";
});

// Error Handling + HSTS
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
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

// ✅ Authentication + Authorization
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
