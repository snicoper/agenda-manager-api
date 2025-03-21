﻿using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Authorization.Errors;

public static class RoleErrors
{
    public static Error RoleNotFound => Error.NotFound(
        code: "RoleErrors.RoleNotFound",
        description: "The role was not found.");

    public static Error RoleNameAlreadyExists => Error.Validation(
        code: nameof(Role.Name),
        description: "The role name already exists.");

    public static Error PermissionAlreadyExistsInRole => Error.Conflict(
        code: "RoleErrors.PermissionAlreadyExistsInRole",
        description: "The permission already exists in the role.");

    public static Error PermissionNotFoundInRole => Error.Conflict(
        code: "RoleErrors.PermissionNotFoundInRole",
        description: "The permission was not found in the role.");

    public static Error RoleIsNotEditable => Error.Conflict(
        code: "RoleErrors.RoleIsNotEditable",
        description: "The role is not editable.");

    public static Error RoleHasUsersAssigned => Error.Conflict(
        code: "RoleErrors.RoleHasUsersAssigned",
        description: "This role has assigned users.");
}
