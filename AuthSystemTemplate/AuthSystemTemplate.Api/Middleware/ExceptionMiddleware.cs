using System.Net;
using System.Text.Json;
using AuthSystemTemplate.Application.Common;

namespace AuthSystemTemplate.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
 
    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
            _logger.LogError(ex, "Unhandled exception occurred. TraceId: {TraceId}", context.TraceIdentifier);
            await HandleExceptionAsync(context, ex);
        }
    }
 
    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, errorCode, message) = exception switch
        {
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized,   "UNAUTHORIZED",   "You are not authorized to perform this action."),
            KeyNotFoundException         => (HttpStatusCode.NotFound,       "NOT_FOUND",      "The requested resource was not found."),
            ArgumentException            => (HttpStatusCode.BadRequest,     "BAD_REQUEST",    "Invalid request."),
            OperationCanceledException   => (HttpStatusCode.BadRequest,     "CANCELLED",      "The request was cancelled."),
            _                            => (HttpStatusCode.InternalServerError, "INTERNAL_ERROR", "An unexpected error occurred.")
        };
 
        var response = new ErrorResponse(
            Code: errorCode,
            Message: message,
            TraceId: context.TraceIdentifier
        );
 
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
 
        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
 
        await context.Response.WriteAsync(json);
    }
}