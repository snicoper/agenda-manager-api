using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Authorization.Errors;

public static class RoleErrors
{
    public static Error RoleNotFound => Error.NotFound("Role not found.");

    public static Error RoleNameAlreadyExists => Error.Validation(nameof(Role.Name), "Role name already exists.");

    public static Error PermissionAlreadyExistsInRole => Error.Conflict("Permission already exists.");

    public static Error PermissionNotFoundInRole => Error.Conflict("Permission not found.");
}
