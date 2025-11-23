using System.Net;
using System.Text.Json;
using Misard.IQs.Application.Exceptions;

namespace Misard.IQs.Api.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, _logger);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex, ILogger logger)
    {
        HttpStatusCode statusCode;
        string errorCode;

        switch (ex)
        {
            case NotFoundException:
                statusCode = HttpStatusCode.NotFound;
                errorCode = "NOT_FOUND";
                break;

            case ValidationException:
                statusCode = HttpStatusCode.BadRequest;
                errorCode = "VALIDATION_ERROR";
                break;

            case BusinessException:
                statusCode = HttpStatusCode.BadRequest;
                errorCode = "BUSINESS_ERROR";
                break;

            default:
                statusCode = HttpStatusCode.InternalServerError;
                errorCode = "UNEXPECTED_ERROR";
                break;
        }

        logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);

        var problem = new
        {
            error = errorCode,
            message = ex.Message
        };

        var payload = JsonSerializer.Serialize(problem);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        return context.Response.WriteAsync(payload);
    }
}
