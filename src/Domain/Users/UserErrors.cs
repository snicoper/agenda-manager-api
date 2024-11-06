﻿using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Users;

public static class UserErrors
{
    public static Error InvalidCredentials => Error.Conflict("Invalid credentials");

    public static Error UserIsNotActive => Error.Conflict("User is not active");

    public static Error UserNotFound => Error.NotFound("User not found");

    public static Error EmailAlreadyExists => Error.Validation(nameof(User.Email), "Email already exists");

    public static Error EmailIsNotConfirmed => Error.Conflict("Email is not confirmed");

    public static Error RoleNotFound => Error.NotFound("Role not found");

    public static Error RoleAlreadyExists => Error.Conflict("Role already exists");

    public static Error UserDoesNotHaveRoleAssigned => Error.Conflict("User does not have role assigned");

    public static Error PermissionNotFound => Error.NotFound("Permission not found");

    public static Error PermissionAlreadyExists => Error.Conflict("Permission already exists");

    public static Error RoleDoesNotHavePermissionAssigned => Error.Conflict("Role does not have permission assigned");

    public static Error InvalidFormatPassword => Error.Validation(
        "Password",
        "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit and one special character");

    public static Error FirstNameExceedsLength => Error.Conflict("First name exceeds length");

    public static Error LastNameExceedsLength => Error.Conflict("Last name exceeds length");
}
