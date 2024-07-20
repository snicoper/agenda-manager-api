using AgendaManager.Application.Common.Interfaces.Users;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;
using AgendaManager.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Infrastructure.Users;

public class AuthorizationManager(AppDbContext context, ICurrentUserProvider currentUserProvider)
    : IAuthorizationManager
{
    public async Task<List<Permission>> GetPermissionsByRoleId(
        RoleId roleId,
        CancellationToken cancellationToken = default)
    {
        var permissions = await context
            .RolePermissions
            .Where(x => x.RoleId == roleId)
            .Select(x => x.Permission)
            .ToListAsync(cancellationToken);

        return permissions;
    }

    public bool HasRole(UserId userId, string role)
    {
        if (currentUserProvider.IsAuthenticated is false)
        {
            return false;
        }

        var currentUser = currentUserProvider.GetCurrentUser();
        var hasRole = currentUser.Roles.Contains(role);

        return hasRole;
    }

    public bool HasPermission(UserId userId, string permissionName)
    {
        if (currentUserProvider.IsAuthenticated is false)
        {
            return false;
        }

        var currentUser = currentUserProvider.GetCurrentUser();
        var hasPermission = currentUser.Permissions.Contains(permissionName);

        return hasPermission;
    }

    public async Task<List<Role>> GetRolesByUserIdAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        var roles = await context
            .UserRoles
            .Where(userRole => userRole.UserId == userId)
            .Select(userRole => userRole.Role)
            .ToListAsync(cancellationToken);

        return roles;
    }

    public async Task<List<User>> GetUsersByRoleIdAsync(RoleId roleId, CancellationToken cancellationToken = default)
    {
        var users = await context
            .UserRoles
            .Where(userRole => userRole.RoleId == roleId)
            .Select(userRole => userRole.User)
            .ToListAsync(cancellationToken);

        return users;
    }

    public async Task AddRoleAsync(UserId userId, RoleId roleId, CancellationToken cancellationToken = default)
    {
        var isInRole = await context
            .UserRoles
            .AnyAsync(r => r.UserId == userId && r.RoleId == roleId, cancellationToken);

        if (isInRole)
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
            .FirstOrDefaultAsync(userRole => userRole.UserId == userId && userRole.RoleId == roleId, cancellationToken);

        if (hasRole is not null)
        {
            context.UserRoles.Remove(hasRole);
        }
    }

    public void AddPermissionToRoleAsync(Role role, Permission permission)
    {
        var rolePermissionExists = context
            .RolePermissions
            .Any(rp => rp.RoleId.Equals(role.Id) && rp.PermissionId.Equals(permission.Id));

        if (rolePermissionExists)
        {
            return;
        }

        var rolePermission = RolePermission.Create(role.Id, permission.Id);
        context.RolePermissions.Add(rolePermission);
    }
}
