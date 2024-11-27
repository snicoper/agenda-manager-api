﻿using AgendaManager.Domain.Authorization.Entities;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Authorization.Errors;

public static class PermissionErrors
{
    public static Error PermissionNotFound => Error.NotFound(
        code: "PermissionErrors.PermissionNotFound",
        description: "Permission not found.");

    public static Error PermissionNameAlreadyExists => Error.Validation(
        code: nameof(Permission.Name),
        description: "Permission name already exists.");
}
