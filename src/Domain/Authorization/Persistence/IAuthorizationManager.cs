﻿using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Authorization.Persistence;

public interface IAuthorizationManager
{
    bool HasRole(UserId userId, string role);

    bool HasPermission(UserId userId, string permissionName);

    Task AddRoleAsync(UserId userId, RoleId roleId, CancellationToken cancellationToken = default);

    Task RemoveRoleAsync(UserId userId, RoleId roleId, CancellationToken cancellationToken = default);

    Task AddPermissionAsync(
        UserId userId,
        PermissionId permissionId,
        CancellationToken cancellationToken = default);

    Task RemovePermissionAsync(
        UserId userId,
        PermissionId permissionId,
        CancellationToken cancellationToken = default);
}
