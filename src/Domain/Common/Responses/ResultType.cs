namespace AgendaManager.Domain.Common.Responses;

public enum ResultType
{
    Succeeded,
    Created,
    NotFound,
    Validation,
    Unauthorized,
    Forbidden,
    Conflict,
    Unexpected
}
