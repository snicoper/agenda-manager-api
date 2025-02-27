﻿using AgendaManager.Domain.Common.Responses;
using Microsoft.AspNetCore.Mvc;

namespace AgendaManager.WebApi.Infrastructure.Results;

public static class ResultExtensions
{
    public static ActionResult ToActionResult(this Result result)
    {
        var statusCode = result.ResultTypeToStatusCodeMapper();

        return statusCode switch
        {
            StatusCodes.Status200OK => new OkObjectResult(ToActionResultResponse(result)),
            StatusCodes.Status201Created => new ObjectResult(ToActionResultResponse(result))
            {
                StatusCode = StatusCodes.Status201Created
            },
            StatusCodes.Status204NoContent => new NoContentResult(),
            StatusCodes.Status400BadRequest => HandleBadRequestResult(result.Error?.ToDictionary()),
            StatusCodes.Status401Unauthorized => new UnauthorizedResult(),
            StatusCodes.Status403Forbidden => new ForbidResult(),
            StatusCodes.Status404NotFound => HandleNotFoundResult(
                result.Error?.FirstError()?.Description,
                result.Error?.FirstError()?.Code),
            StatusCodes.Status409Conflict => HandleConflictResult(
                result.Error?.FirstError()?.Description,
                result.Error?.FirstError()?.Code),
            _ => HandleUnexpectedResult(statusCode, result.Error?.FirstError())
        };
    }

    public static ActionResult<Result<TValue>> ToActionResult<TValue>(this Result<TValue> result)
    {
        var statusCode = result.ResultTypeToStatusCodeMapper();

        return statusCode switch
        {
            StatusCodes.Status200OK => new OkObjectResult(ToActionResultResponse(result)),
            StatusCodes.Status201Created => new ObjectResult(ToActionResultResponse(result))
            {
                StatusCode = StatusCodes.Status201Created
            },
            StatusCodes.Status204NoContent => new NoContentResult(),
            StatusCodes.Status400BadRequest => HandleBadRequestResult(result.Error?.ToDictionary()),
            StatusCodes.Status401Unauthorized => new UnauthorizedResult(),
            StatusCodes.Status403Forbidden => new ForbidResult(),
            StatusCodes.Status404NotFound => HandleNotFoundResult(
                result.Error?.FirstError()?.Description,
                result.Error?.FirstError()?.Code),
            StatusCodes.Status409Conflict => HandleConflictResult(
                result.Error?.FirstError()?.Description,
                result.Error?.FirstError()?.Code),
            _ => HandleUnexpectedResult(statusCode, result.Error?.FirstError())
        };
    }

    private static int ResultTypeToStatusCodeMapper(this Result result)
    {
        return result.ResultType switch
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

    private static BadRequestObjectResult HandleBadRequestResult(IDictionary<string, string[]>? errors)
    {
        ValidationProblemDetails validationProblemDetails = new()
        {
            Status = StatusCodes.Status400BadRequest,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Errors = errors ?? new Dictionary<string, string[]>()
        };

        return new BadRequestObjectResult(validationProblemDetails);
    }

    private static NotFoundObjectResult HandleNotFoundResult(string? description, string? code)
    {
        CustomProblemDetails problemDetails = new()
        {
            Status = StatusCodes.Status404NotFound,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Title = "The specified resource was not found.",
            Code = code,
            Detail = description
        };

        return new NotFoundObjectResult(problemDetails);
    }

    private static ConflictObjectResult HandleConflictResult(string? description, string? code)
    {
        CustomProblemDetails problemDetails = new()
        {
            Status = StatusCodes.Status409Conflict,
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.8",
            Title = "The request could not be completed due to a conflict.",
            Code = code,
            Detail = description
        };

        return new ConflictObjectResult(problemDetails);
    }

    private static ObjectResult HandleUnexpectedResult(int statusCode, ValidationError? validationError)
    {
        return new ObjectResult(
            new CustomProblemDetails
            {
                Status = statusCode,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Title = validationError?.Code,
                Detail = validationError?.Description
            });
    }

    private static ResultResponse ToActionResultResponse(Result result)
    {
        return new ResultResponse(result.IsSuccess, result.ResultType);
    }

    private static ResultValueResponse<TValue> ToActionResultResponse<TValue>(Result<TValue> result)
    {
        return new ResultValueResponse<TValue>(result.Value, result.IsSuccess, result.ResultType);
    }
}
