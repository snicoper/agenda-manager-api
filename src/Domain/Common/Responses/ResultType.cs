namespace AgendaManager.Domain.Common.Responses;

public enum ResultType
{
    Succeeded,
    Created,
    NotFound,
    ValidationError,
    Unauthorized,
    Forbidden,
    Conflict,
    InternalServerError
}
