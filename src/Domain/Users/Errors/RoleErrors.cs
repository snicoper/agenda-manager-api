using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Entities;

namespace AgendaManager.Domain.Users.Errors;

public static class RoleErrors
{
    public static Error RoleNotFound => Error.NotFound("Role not found.");

    public static Error RoleNameAlreadyExists => Error.Validation(nameof(Role.Name), "Role name already exists.");
}
