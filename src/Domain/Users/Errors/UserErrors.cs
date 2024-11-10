using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Aggregates;

namespace AgendaManager.Domain.Users.Errors;

public static class UserErrors
{
    public static Error InvalidCredentials => Error.Conflict("Invalid credentials.");

    public static Error UserIsNotActive => Error.Conflict("User is not active.");

    public static Error UserNotFound => Error.NotFound("User not found.");

    public static Error EmailAlreadyExists => Error.Validation(nameof(User.Email), "Email already exists.");

    public static Error EmailIsNotConfirmed => Error.Conflict("Email is not confirmed.");

    public static Error UserDoesNotHaveRoleAssigned => Error.Conflict("User does not have role assigned.");

    public static Error InvalidFormatPassword => Error.Validation(
        "Password",
        "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit and one special character.");
}
