using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.WebApi.Extensions;

public static class ResultExtensions
{
    public static int ResultTypeToStatusCodeMapper(this Result result)
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

    public static int ResultTypeToStatusCodeMapper<TValue>(this Result<TValue> result)
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
}
