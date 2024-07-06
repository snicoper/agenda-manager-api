using AgendaManager.Domain.Common.Responses;
using AgendaManager.WebApi.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace AgendaManager.WebApi.Extensions;

public static class ControllerExtensions
{
    public static ActionResult<Result> ToHttpResponse(this ControllerBase controllerBase, Result result)
    {
        var statusCode = result.ResultTypeToStatusCodeMapper();

        controllerBase.HttpContext.Response.StatusCode = statusCode;

        return statusCode switch
        {
            StatusCodes.Status200OK => new ObjectResult(result) { StatusCode = statusCode },
            StatusCodes.Status201Created => new ObjectResult(result) { StatusCode = StatusCodes.Status201Created },
            StatusCodes.Status204NoContent => controllerBase.NoContent(),
            StatusCodes.Status400BadRequest => HandleBadRequestResult(controllerBase, result.Error?.ValidationErrors),
            StatusCodes.Status401Unauthorized => controllerBase.Unauthorized(),
            StatusCodes.Status403Forbidden => controllerBase.Forbid(),
            StatusCodes.Status404NotFound => HandleNotFoundResult(controllerBase, result.Error?.Description),
            _ => HandleUnexpectedResult(controllerBase, statusCode, result.Error?.Code, result.Error?.Description)
        };
    }

    public static ActionResult<Result<TValue>> ToHttpResponse<TValue>(this ControllerBase controllerBase, Result<TValue> result)
    {
        var statusCode = result.ResultTypeToStatusCodeMapper();

        controllerBase.HttpContext.Response.StatusCode = statusCode;

        return statusCode switch
        {
            StatusCodes.Status200OK => new ObjectResult(result) { StatusCode = statusCode },
            StatusCodes.Status201Created => new ObjectResult(result) { StatusCode = StatusCodes.Status201Created },
            StatusCodes.Status204NoContent => controllerBase.NoContent(),
            StatusCodes.Status400BadRequest => HandleBadRequestResult(controllerBase, result.Error?.ValidationErrors),
            StatusCodes.Status401Unauthorized => controllerBase.Unauthorized(),
            StatusCodes.Status403Forbidden => controllerBase.Forbid(),
            StatusCodes.Status404NotFound => HandleNotFoundResult(controllerBase, result.Error?.Description),
            _ => HandleUnexpectedResult(controllerBase, statusCode, result.Error?.Code, result.Error?.Description)
        };
    }

    private static BadRequestObjectResult HandleBadRequestResult(
        ControllerBase controllerBase,
        IDictionary<string, string[]>? errors)
    {
        if (errors is null)
        {
            return new BadRequestObjectResult(nameof(controllerBase.BadRequest));
        }

        var validationProblemDetails = new ValidationProblemDetails(errors);

        return new BadRequestObjectResult(validationProblemDetails);
    }

    private static NotFoundObjectResult HandleNotFoundResult(ControllerBase controllerBase, string? description)
    {
        ProblemDetails problemDetails = new()
        {
            Status = StatusCodes.Status404NotFound,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Title = "The specified resource was not found.",
            Detail = description
        };

        return controllerBase.NotFound(problemDetails);
    }

    private static ObjectResult HandleUnexpectedResult(
        ControllerBase controllerBase,
        int statusCode,
        string? code,
        string? description)
    {
        var log = controllerBase.HttpContext
            .RequestServices
            .GetRequiredService<ILogger<ApiControllerBase>>();

        log.LogError("Validation errors in {Type} with values:{Code} - {@Error}", controllerBase.GetType(), code, description);

        return controllerBase.Problem(statusCode: statusCode, detail: description);
    }
}
