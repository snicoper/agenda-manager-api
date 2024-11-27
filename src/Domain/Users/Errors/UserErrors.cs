using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Users.Errors;

public static class UserErrors
{
    public static Error InvalidCredentials => Error.Conflict(
        description: "Invalid credentials.",
        code: "UserErrors.InvalidCredentials");

    public static Error UserIsNotActive => Error.Conflict(
        description: "User is not active.",
        code: "UserErrors.UserIsNotActive");

    public static Error UserNotFound => Error.NotFound(
        code: "UserErrors.UserNotFound",
        description: "User not found.");

    public static Error EmailAlreadyExists => Error.Validation(
        code: nameof(User.Email),
        description: "Email already exists.");

    public static Error EmailIsNotConfirmed => Error.Conflict(
        code: "UserErrors.EmailIsNotConfirmed",
        description: "Email is not confirmed.");

    public static Error InvalidFormatPassword => Error.Validation(
        code: "Password",
        "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit and one special character.");

    public static Error RoleAlreadyExists => Error.Conflict(
        code: "UserErrors.RoleAlreadyExists",
        description: "Role already exists.");

    public static Error RoleDoesNotExist => Error.Conflict(
        code: "UserErrors.RoleDoesNotExist",
        description: "Role does not exist.");

    public static Error UserAlreadyConfirmedEmail => Error.Conflict(
        code: "UserErrors.EmailIsConfirmed",
        description: "User already confirmed email.");
}
