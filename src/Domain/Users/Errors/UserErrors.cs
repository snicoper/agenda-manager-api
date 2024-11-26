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
        description: "User not found.",
        code: "UserErrors.UserNotFound");

    public static Error EmailAlreadyExists => Error.Validation(nameof(User.Email), "Email already exists.");

    public static Error EmailIsNotConfirmed => Error.Conflict(
        description: "Email is not confirmed.",
        code: "UserErrors.EmailIsNotConfirmed");

    public static Error InvalidFormatPassword => Error.Validation(
        code: "Password",
        "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit and one special character.");

    public static Error RoleAlreadyExists => Error.Conflict(
        description: "Role already exists.",
        code: "UserErrors.RoleAlreadyExists");

    public static Error RoleDoesNotExist => Error.Conflict(
        description: "Role does not exist.",
        code: "UserErrors.RoleDoesNotExist");

    public static Error UserAlreadyConfirmedEmail => Error.Conflict(
        description: "User already confirmed email.",
        code: "UserErrors.EmailIsConfirmed");
}
