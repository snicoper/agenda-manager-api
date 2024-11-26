﻿using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Users.Errors;

public static class UserTokenErrors
{
    public static Error InvalidToken => Error.Conflict("User token is invalid.");

    public static Error TokenHasExpired => Error.Conflict("Token has expired.");

    public static Error UserTokenNotFound => Error.NotFound("User token not found.");

    public static Error UserTokenNotFoundOrExpired => Error.Conflict("User token was not found or has expired.");
}
