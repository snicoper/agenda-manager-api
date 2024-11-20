using AgendaManager.Domain.Authorization.Entities;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Authorization.Errors;

public static class PermissionErrors
{
    public static Error PermissionNotFound => Error.NotFound("Permission not found.");

    public static Error PermissionNameAlreadyExists => Error.Validation(
        nameof(Permission.Name),
        "Permission name already exists.");
}
