using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Users.Errors;

public static class UserTokenErrors
{
    public static Error InvalidToken => Error.Conflict(
        code: "UserTokenErrors.InvalidToken",
        description: "The user token is invalid.");

    public static Error TokenHasExpired => Error.Conflict(
        code: "UserTokenErrors.TokenHasExpired",
        description: "The user token has expired.");

    public static Error UserTokenNotFound => Error.NotFound(
        code: "UserTokenErrors.UserTokenNotFound",
        description: "The user token was not found.");

    public static Error UserTokenNotFoundOrExpired => Error.Conflict(
        code: "UserTokenErrors.UserTokenNotFoundOrExpired",
        description: "The user token was not found or has expired.");
}
