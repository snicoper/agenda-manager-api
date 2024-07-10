using AgendaManager.Application.Common.Interfaces.Users;
using AgendaManager.Domain.Authorization;
using AgendaManager.Domain.Authorization.Persistence;
using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Users.ValueObjects;
using AgendaManager.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Infrastructure.Authorization;

public class AuthorizationManager(AppDbContext context, ICurrentUserProvider currentUserProvider)
    : IAuthorizationManager
{
    public bool HasRole(UserId userId, string role)
    {
        if (currentUserProvider.IsAuthenticated is false)
        {
            return false;
        }

        var currentUser = currentUserProvider.GetCurrentUser();
        var havePermission = currentUser.Roles.Contains(role);

        return havePermission;
    }

    public bool HasPermission(UserId userId, string permissionName)
    {
        if (currentUserProvider.IsAuthenticated is false)
        {
            return false;
        }

        var currentUser = currentUserProvider.GetCurrentUser();
        var havePermission = currentUser.Permissions.Contains(permissionName);

        return havePermission;
    }

    public async Task<List<Role>> GetRolesByUserIdAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        var roles = await context
            .UserRoles
            .Where(r => r.UserId == userId)
            .Select(r => r.Role)
            .ToListAsync(cancellationToken);

        return roles;
    }

    public Task<List<Permission>> GetPermissionsByUserIdAsync(
        UserId userId,
        CancellationToken cancellationToken = default)
    {
        var permissions = context
            .UserPermissions
            .Where(r => r.UserId == userId)
            .Select(r => r.Permission)
            .ToListAsync(cancellationToken);

        return permissions;
    }

    public async Task AddRoleAsync(UserId userId, RoleId roleId, CancellationToken cancellationToken = default)
    {
        var isInRole = await context
            .UserRoles
            .FirstOrDefaultAsync(r => r.UserId == userId && r.RoleId == roleId, cancellationToken);

        if (isInRole is not null)
        {
            return;
        }

        var userRole = UserRole.Create(userId, roleId);

        context.UserRoles.Add(userRole);
    }

    public async Task RemoveRoleAsync(
        UserId userId,
        RoleId roleId,
        CancellationToken cancellationToken = default)
    {
        var hasRole = await context
            .UserRoles
            .FirstOrDefaultAsync(r => r.UserId == userId && r.RoleId == roleId, cancellationToken);

        if (hasRole is not null)
        {
            context.UserRoles.Remove(hasRole);
        }
    }

    public async Task AddPermissionAsync(
        UserId userId,
        PermissionId permissionId,
        CancellationToken cancellationToken = default)
    {
        var isInPermission = await context
            .UserPermissions
            .FirstOrDefaultAsync(r => r.UserId == userId && r.PermissionId == permissionId, cancellationToken);

        if (isInPermission is not null)
        {
            return;
        }

        var userPermission = UserPermission.Create(userId, permissionId);

        context.UserPermissions.Add(userPermission);
    }

    public async Task RemovePermissionAsync(
        UserId userId,
        PermissionId permissionId,
        CancellationToken cancellationToken = default)
    {
        var hasPermission = await context
            .UserPermissions
            .FirstOrDefaultAsync(r => r.UserId == userId && r.PermissionId == permissionId, cancellationToken);

        if (hasPermission is not null)
        {
            context.UserPermissions.Remove(hasPermission);
        }
    }
}
