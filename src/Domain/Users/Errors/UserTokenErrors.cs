using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Users.Errors;

public static class UserTokenErrors
{
    public static Error InvalidToken => Error.Conflict("User token is invalid.");

    public static Error TokenHasExpired => Error.Conflict("Token has expired.");
}
