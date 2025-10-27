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
            _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, response) = exception switch
        {
            ValidationException validationEx => (
                (int)HttpStatusCode.BadRequest,
                ApiResponse<object>.Fail(
                    string.Join("; ", validationEx.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"))
                )
            ),
            UnauthorizedAccessException unauthorizedEx => (
                (int)HttpStatusCode.Unauthorized,
                ApiResponse<object>.Fail(unauthorizedEx.Message)
            ),
            KeyNotFoundException notFoundEx => (
                (int)HttpStatusCode.NotFound,
                ApiResponse<object>.Fail(notFoundEx.Message)
            ),
            ArgumentException argEx => (
                (int)HttpStatusCode.BadRequest,
                ApiResponse<object>.Fail(argEx.Message)
            ),
            InvalidOperationException invalidOpEx => (
                (int)HttpStatusCode.BadRequest,
                ApiResponse<object>.Fail(invalidOpEx.Message)
            ),
            _ => (
                (int)HttpStatusCode.InternalServerError,
                ApiResponse<object>.Fail("Có l?i x?y ra trong h? th?ng. Vui lòng th? l?i sau.")
            )
        };

        context.Response.StatusCode = statusCode;

        var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }
}