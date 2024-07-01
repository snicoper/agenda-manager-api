using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.WebApi.Constants;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgendaManager.WebApi.Infrastructure;

[ApiController]
[Authorize]
[ApiVersion(ApiVersions.V1)]
[Produces("application/json")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class ApiControllerBase : ControllerBase
{
    private ISender? _sender;

    protected ISender Sender => _sender ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    protected ActionResult<Result> HandleResult(Result result)
    {
        HttpContext.Response.StatusCode = result.Status;

        return result.Status switch
        {
            StatusCodes.Status200OK => new ObjectResult(result) { StatusCode = result.Status },
            StatusCodes.Status201Created => new ObjectResult(result) { StatusCode = StatusCodes.Status201Created },
            StatusCodes.Status204NoContent => NoContent(),
            StatusCodes.Status400BadRequest => HandleBadRequestResult(result.Error?.ValidationErrors),
            StatusCodes.Status401Unauthorized => Unauthorized(),
            StatusCodes.Status403Forbidden => Forbid(),
            StatusCodes.Status404NotFound => HandleNotFoundResult(result.Error?.Description),
            _ => HandleUnknownResult(result.Status, result.Error?.Description)
        };
    }

    protected ActionResult<Result<TValue>> HandleResult<TValue>(Result<TValue> result)
    {
        HttpContext.Response.StatusCode = result.Status;

        return result.Status switch
        {
            StatusCodes.Status200OK => new ObjectResult(result) { StatusCode = result.Status },
            StatusCodes.Status201Created => new ObjectResult(result) { StatusCode = StatusCodes.Status201Created },
            StatusCodes.Status204NoContent => NoContent(),
            StatusCodes.Status400BadRequest => HandleBadRequestResult(result.Error?.ValidationErrors),
            StatusCodes.Status401Unauthorized => Unauthorized(),
            StatusCodes.Status403Forbidden => Forbid(),
            StatusCodes.Status404NotFound => HandleNotFoundResult(result.Error?.Description),
            _ => HandleUnknownResult(result.Status, result.Error?.Description)
        };
    }

    private BadRequestObjectResult HandleBadRequestResult(IDictionary<string, string[]>? errors)
    {
        if (errors is null)
        {
            return new BadRequestObjectResult(nameof(BadRequest));
        }

        var resultResponse = new ValidationProblemDetails(errors);

        return new BadRequestObjectResult(resultResponse);
    }

    private NotFoundObjectResult HandleNotFoundResult(string? description)
    {
        ProblemDetails problemDetails = new()
        {
            Status = StatusCodes.Status404NotFound,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Title = "The specified resource was not found.",
            Detail = description
        };

        return NotFound(problemDetails);
    }

    private ObjectResult HandleUnknownResult(int statusCode, string? description)
    {
        var log = HttpContext
            .RequestServices
            .GetRequiredService<ILogger<ApiControllerBase>>();

        log.LogError("Validation errors in {Type} with values: {@Error}", GetType(), description);

        return Problem(statusCode: statusCode, detail: description);
    }
}
