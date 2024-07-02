namespace AgendaManager.Domain.Common.Responses;

public enum ErrorType
{
    None,
    NotFound,
    ValidationError,
    Unauthorized,
    Forbidden,
    Conflict,
    InternalServerError
}
