using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Users.Errors;

public static class UserErrors
{
    public static Error InvalidCredentials => Error.Conflict(
        code: "UserErrors.InvalidCredentials",
        description: "The provided credentials are invalid.");

    public static Error UserIsNotActive => Error.Conflict(
        code: "UserErrors.UserIsNotActive",
        description: "The user is not active.");

    public static Error UserNotFound => Error.NotFound(
        code: "UserErrors.UserNotFound",
        description: "The user was not found.");

    public static Error EmailAlreadyExists => Error.Validation(
        code: nameof(User.Email),
        description: "The email already exists.");

    public static Error EmailIsNotConfirmed => Error.Conflict(
        code: "UserErrors.EmailIsNotConfirmed",
        description: "The email is not confirmed.");

    public static Error InvalidFormatPassword => Error.Validation(
        code: "Password",
        "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit and one special character.");

    public static Error RoleAlreadyExists => Error.Conflict(
        code: "UserErrors.RoleAlreadyExists",
        description: "The user already has the specified role.");

    public static Error RoleDoesNotExist => Error.Conflict(
        code: "UserErrors.RoleDoesNotExist",
        description: "The user does not have the specified role.");

    public static Error UserAlreadyConfirmedEmail => Error.Conflict(
        code: "UserErrors.EmailIsConfirmed",
        description: "The user has already confirmed their email.");

    public static Error UserRoleNotFound => Error.NotFound(
        code: "UserErrors.UserRoleNotFound",
        description: "The user role was not found.");
}
