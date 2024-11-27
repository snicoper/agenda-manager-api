using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Authorization.Errors;

public static class RoleErrors
{
    public static Error RoleNotFound => Error.NotFound(
        code: "RoleErrors.RoleNotFound",
        description: "Role not found.");

    public static Error RoleNameAlreadyExists => Error.Validation(
        code: nameof(Role.Name),
        description: "Role name already exists.");

    public static Error PermissionAlreadyExistsInRole => Error.Conflict(
        code: "RoleErrors.PermissionAlreadyExistsInRole",
        description: "Permission already exists.");

    public static Error PermissionNotFoundInRole => Error.Conflict(
        code: "RoleErrors.PermissionNotFoundInRole",
        description: "Permission not found.");
}
