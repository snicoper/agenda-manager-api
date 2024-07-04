﻿using AgendaManager.Domain.Common.Responses;
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

    protected ActionResult<Result> ToHttpResponse(Result result)
    {
        var statusCode = ResultTypeToStatusCodeMapper(result.ResultType);

        HttpContext.Response.StatusCode = statusCode;

        return statusCode switch
        {
            StatusCodes.Status200OK => new ObjectResult(result) { StatusCode = statusCode },
            StatusCodes.Status201Created => new ObjectResult(result) { StatusCode = StatusCodes.Status201Created },
            StatusCodes.Status204NoContent => NoContent(),
            StatusCodes.Status400BadRequest => HandleBadRequestResult(result.Error?.ValidationErrors),
            StatusCodes.Status401Unauthorized => Unauthorized(),
            StatusCodes.Status403Forbidden => Forbid(),
            StatusCodes.Status404NotFound => HandleNotFoundResult(result.Error?.Description),
            _ => HandleUnexpectedResult(statusCode, result.Error?.Code, result.Error?.Description)
        };
    }

    protected ActionResult<Result<TValue>> ToHttpResponse<TValue>(Result<TValue> result)
    {
        var statusCode = ResultTypeToStatusCodeMapper(result.ResultType);

        HttpContext.Response.StatusCode = statusCode;

        return statusCode switch
        {
            StatusCodes.Status200OK => new ObjectResult(result) { StatusCode = statusCode },
            StatusCodes.Status201Created => new ObjectResult(result) { StatusCode = StatusCodes.Status201Created },
            StatusCodes.Status204NoContent => NoContent(),
            StatusCodes.Status400BadRequest => HandleBadRequestResult(result.Error?.ValidationErrors),
            StatusCodes.Status401Unauthorized => Unauthorized(),
            StatusCodes.Status403Forbidden => Forbid(),
            StatusCodes.Status404NotFound => HandleNotFoundResult(result.Error?.Description),
            _ => HandleUnexpectedResult(statusCode, result.Error?.Code, result.Error?.Description)
        };
    }

    private static int ResultTypeToStatusCodeMapper(ResultType resultType)
    {
        return resultType switch
        {
            ResultType.Succeeded => StatusCodes.Status200OK,
            ResultType.Created => StatusCodes.Status201Created,
            ResultType.NoContent => StatusCodes.Status204NoContent,
            ResultType.NotFound => StatusCodes.Status404NotFound,
            ResultType.Validation => StatusCodes.Status400BadRequest,
            ResultType.Unauthorized => StatusCodes.Status401Unauthorized,
            ResultType.Forbidden => StatusCodes.Status403Forbidden,
            ResultType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };
    }

    private BadRequestObjectResult HandleBadRequestResult(IDictionary<string, string[]>? errors)
    {
        if (errors is null)
        {
            return new BadRequestObjectResult(nameof(BadRequest));
        }

        var validationProblemDetails = new ValidationProblemDetails(errors);

        return new BadRequestObjectResult(validationProblemDetails);
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

    private ObjectResult HandleUnexpectedResult(int statusCode, string? code, string? description)
    {
        var log = HttpContext
            .RequestServices
            .GetRequiredService<ILogger<ApiControllerBase>>();

        log.LogError("Validation errors in {Type} with values:{Code} - {@Error}", GetType(), code, description);

        return Problem(statusCode: statusCode, detail: description);
    }
}
