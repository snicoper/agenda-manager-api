using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Users.Errors;

public static class PermissionErrors
{
    public static Error PermissionNotFound => Error.NotFound("Permission not found.");

    public static Error PermissionAlreadyExists => Error.Conflict("Permission already exists.");

    public static Error PermissionNameExceedsLength => Error.Validation(
        nameof(Permission.Name),
        "Permission name exceeds length.");

    public static Error PermissionNameAlreadyExists => Error.Validation(
        nameof(Permission.Name),
        "Permission name already exists.");
}
