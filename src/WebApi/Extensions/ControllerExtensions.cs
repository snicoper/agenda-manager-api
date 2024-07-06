using AgendaManager.Domain.Common.Responses;
using AgendaManager.WebApi.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace AgendaManager.WebApi.Extensions;

public static class ControllerExtensions
{
    public static ActionResult<Result> ToHttpResponse(this ControllerBase controller, Result result)
    {
        var statusCode = result.ResultTypeToStatusCodeMapper();

        controller.HttpContext.Response.StatusCode = statusCode;

        return statusCode switch
        {
            StatusCodes.Status200OK => new ObjectResult(result) { StatusCode = statusCode },
            StatusCodes.Status201Created => new ObjectResult(result) { StatusCode = StatusCodes.Status201Created },
            StatusCodes.Status204NoContent => controller.NoContent(),
            StatusCodes.Status400BadRequest => HandleBadRequestResult(result.Error?.ValidationErrors),
            StatusCodes.Status401Unauthorized => controller.Unauthorized(),
            StatusCodes.Status403Forbidden => controller.Forbid(),
            StatusCodes.Status404NotFound => HandleNotFoundResult(controller, result.Error?.Description),
            _ => HandleUnexpectedResult(controller, statusCode, result.Error?.Code, result.Error?.Description)
        };
    }

    public static ActionResult<Result<TValue>> ToHttpResponse<TValue>(this ControllerBase controller, Result<TValue> result)
    {
        var statusCode = result.ResultTypeToStatusCodeMapper();

        controller.HttpContext.Response.StatusCode = statusCode;

        return statusCode switch
        {
            StatusCodes.Status200OK => new ObjectResult(result) { StatusCode = statusCode },
            StatusCodes.Status201Created => new ObjectResult(result) { StatusCode = StatusCodes.Status201Created },
            StatusCodes.Status204NoContent => controller.NoContent(),
            StatusCodes.Status400BadRequest => HandleBadRequestResult(result.Error?.ValidationErrors),
            StatusCodes.Status401Unauthorized => controller.Unauthorized(),
            StatusCodes.Status403Forbidden => controller.Forbid(),
            StatusCodes.Status404NotFound => HandleNotFoundResult(controller, result.Error?.Description),
            _ => HandleUnexpectedResult(controller, statusCode, result.Error?.Code, result.Error?.Description)
        };
    }

    private static BadRequestObjectResult HandleBadRequestResult(IDictionary<string, string[]>? errors)
    {
        if (errors is null)
        {
            return new BadRequestObjectResult("BadRequest");
        }

        var validationProblemDetails = new ValidationProblemDetails(errors);

        return new BadRequestObjectResult(validationProblemDetails);
    }

    private static NotFoundObjectResult HandleNotFoundResult(ControllerBase controller, string? description)
    {
        ProblemDetails problemDetails = new()
        {
            Status = StatusCodes.Status404NotFound,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Title = "The specified resource was not found.",
            Detail = description
        };

        return controller.NotFound(problemDetails);
    }

    private static ObjectResult HandleUnexpectedResult(
        ControllerBase controller,
        int statusCode,
        string? code,
        string? description)
    {
        var log = controller.HttpContext
            .RequestServices
            .GetRequiredService<ILogger<ApiControllerBase>>();

        log.LogError("Error in {Type}: {Code} - {@Error}", controller.GetType(), code, description);

        return controller.Problem(statusCode: statusCode, detail: description);
    }
}
