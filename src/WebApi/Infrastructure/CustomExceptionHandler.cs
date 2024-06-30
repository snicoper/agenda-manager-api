using AgendaManager.Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AgendaManager.WebApi.Infrastructure;

public class CustomExceptionHandler : IExceptionHandler
{
    // Register known exception types and handlers.
    private readonly Dictionary<Type, Func<HttpContext, Exception, Task>> _exceptionHandlers = new()
    {
        { typeof(BadRequestException), HandleValidationException },
        { typeof(NotFoundException), HandleNotFoundException },
        { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
        { typeof(ForbiddenAccessException), HandleForbiddenAccessException }
    };

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        Type exceptionType = exception.GetType();

        if (!_exceptionHandlers.TryGetValue(exceptionType, out Func<HttpContext, Exception, Task>? handler))
        {
            await HandleUnknownException(httpContext, exception);

            return false;
        }

        await handler.Invoke(httpContext, exception);

        return true;
    }

    private static async Task HandleValidationException(HttpContext httpContext, Exception exception)
    {
        BadRequestException badRequestException = (BadRequestException)exception;

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        ValidationProblemDetails errors = new(badRequestException.Errors)
        {
            Status = StatusCodes.Status400BadRequest, Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
        };

        await httpContext.Response.WriteAsJsonAsync(errors);
    }

    private static async Task HandleNotFoundException(HttpContext httpContext, Exception exception)
    {
        NotFoundException notFoundException = (NotFoundException)exception;

        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Title = "The specified resource was not found.",
                Detail = notFoundException.Message
            });
    }

    private static async Task HandleUnauthorizedAccessException(HttpContext httpContext, Exception exception)
    {
        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
                Type = "https://tools.ietf.org/html/rfc7235#section-3.1",
                Detail = GetDetailsExceptionByEnvironment(httpContext, exception)
            });
    }

    private static async Task HandleForbiddenAccessException(HttpContext httpContext, Exception exception)
    {
        httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;

        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Status = StatusCodes.Status403Forbidden,
                Title = "Forbidden",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3",
                Detail = GetDetailsExceptionByEnvironment(httpContext, exception)
            });
    }

    private static async Task HandleUnknownException(HttpContext httpContext, Exception exception)
    {
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(
            new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An error occurred while processing your request.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Detail = GetDetailsExceptionByEnvironment(httpContext, exception)
            });
    }

    private static string GetDetailsExceptionByEnvironment(HttpContext httpContext, Exception exception)
    {
        IWebHostEnvironment hostEnvironment = httpContext
            .RequestServices
            .GetRequiredService<IWebHostEnvironment>();

        return !hostEnvironment.IsProduction() ? exception.Message : string.Empty;
    }
}
