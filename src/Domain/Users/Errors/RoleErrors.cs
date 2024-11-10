using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Aggregates;

namespace AgendaManager.Domain.Users.Errors;

public static class RoleErrors
{
    public static Error RoleNotFound => Error.NotFound("Role not found.");

    public static Error RoleAlreadyExists => Error.Conflict("Role already exists.");

    public static Error RoleDoesNotHavePermissionAssigned => Error.Conflict("Role does not have permission assigned.");

    public static Error RoleNameAlreadyExists => Error.Validation(nameof(Role.Name), "Role name already exists.");
}
