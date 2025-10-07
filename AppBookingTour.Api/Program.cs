using AppBookingTour.Infrastructure;
using AppBookingTour.Share.Configurations;
using Microsoft.OpenApi.Models;
using Serilog;

// Configure Serilog for structured logging
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true, reloadOnChange: true)
        .Build())
    .Enrich.FromLogContext()
    .CreateLogger();

Log.Information("Starting AppBookingTour API application");

var builder = WebApplication.CreateBuilder(args);

// Add Serilog
builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddControllers();

// Infrastructure Layer (Db, Identity, Repositories, External services)
builder.Services.AddInfrastructure(builder.Configuration);

// Health Checks (basic)
builder.Services.AddHealthChecks();

// Output cache for controller attributes
builder.Services.AddOutputCache();

// Bind configuration objects
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

// CORS
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

// Swagger
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

    // JWT in Swagger
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

var app = builder.Build();

// Always enable Swagger (both Development & Production)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AppBookingTour API V1");
    // Serve Swagger UI at /swagger and keep root redirect
    c.RoutePrefix = "swagger"; 
});

// Environment specific handlers
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Security headers (basic)
app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "DENY";
    context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
    await next();
});

app.UseHttpsRedirection();

// Logging of requests
app.UseSerilogRequestLogging();

// CORS
app.UseCors("AllowSpecificOrigins");

app.UseRouting();

// Output cache must be added before endpoints
app.UseOutputCache();

// AuthN/AuthZ
app.UseAuthentication();
app.UseAuthorization();

// Map endpoints
app.MapControllers();

// Health checks endpoint
app.MapHealthChecks("/health");

// Root redirect to Swagger UI
app.MapGet("/", () => Results.Redirect("/swagger"));

Log.Information("Starting web host on {Environment}", app.Environment.EnvironmentName);
await app.RunAsync();await app.RunAsync();