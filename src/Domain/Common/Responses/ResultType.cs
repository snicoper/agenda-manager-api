namespace AgendaManager.Domain.Common.Responses;

public enum ResultType
{
    Succeeded = 200,
    Created = 201,
    NoContent = 204,
    Validation = 400,
    Unauthorized = 401,
    Forbidden = 403,
    NotFound = 404,
    Conflict = 409,
    Unexpected = 500
}
