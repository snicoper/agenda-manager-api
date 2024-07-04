namespace AgendaManager.Domain.Common.Responses;

public enum ResultType
{
    Succeeded,
    Created,
    NotFound,
    NoContent,
    Validation,
    Unauthorized,
    Forbidden,
    Conflict,
    Unexpected
}
