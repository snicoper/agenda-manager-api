using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Users.Errors;

public static class UserTokenErrors
{
    public static Error InvalidToken => Error.Conflict(
        code: "UserTokenErrors.InvalidToken",
        description: "User token is invalid.");

    public static Error TokenHasExpired => Error.Conflict(
        code: "UserTokenErrors.TokenHasExpired",
        description: "Token has expired.");

    public static Error UserTokenNotFound => Error.NotFound(
        code: "UserTokenErrors.UserTokenNotFound",
        description: "User token not found.");

    public static Error UserTokenNotFoundOrExpired => Error.Conflict(
        code: "UserTokenErrors.UserTokenNotFoundOrExpired",
        description: "User token was not found or has expired.");
}
