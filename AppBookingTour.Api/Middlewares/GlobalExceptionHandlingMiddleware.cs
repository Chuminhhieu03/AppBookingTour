using AppBookingTour.Api.Contracts.Responses;
using FluentValidation;
using System.Net;
using System.Text.Json;

namespace AppBookingTour.Api.Middlewares;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = exception switch
        {
            ValidationException validationEx => new ApiResponse<object>
            {
                Success = false,
                Message = "Validation failed",
                Data = validationEx.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
            },
            UnauthorizedAccessException => new ApiResponse<object>
            {
                Success = false,
                Message = "Unauthorized access"
            },
            KeyNotFoundException => new ApiResponse<object>
            {
                Success = false,
                Message = "Resource not found"
            },
            ArgumentException argEx => new ApiResponse<object>
            {
                Success = false,
                Message = argEx.Message
            },
            _ => new ApiResponse<object>
            {
                Success = false,
                Message = "An internal server error occurred"
            }
        };

        context.Response.StatusCode = exception switch
        {
            ValidationException => (int)HttpStatusCode.BadRequest,
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
            KeyNotFoundException => (int)HttpStatusCode.NotFound,
            ArgumentException => (int)HttpStatusCode.BadRequest,
            _ => (int)HttpStatusCode.InternalServerError
        };

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }
}