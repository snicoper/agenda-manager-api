using AgendaManager.Domain.Authorization.Entities;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Authorization.Errors;

public static class PermissionErrors
{
    public static Error PermissionNotFound => Error.NotFound(
        code: "PermissionErrors.PermissionNotFound",
        description: "The permission was not found.");

    public static Error PermissionNameAlreadyExists => Error.Validation(
        code: nameof(Permission.Name),
        description: "The permission name already exists.");
}
