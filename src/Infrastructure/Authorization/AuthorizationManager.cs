using AgendaManager.Application.Common.Interfaces.Users;
using AgendaManager.Application.Common.Models.Users;
using AgendaManager.Domain.Authorization;
using AgendaManager.Domain.Authorization.Persistence;
using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.ValueObjects;
using AgendaManager.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Infrastructure.Authorization;

public class AuthorizationManager(AppDbContext context, ICurrentUserProvider currentUserProvider)
    : IAuthorizationManager
{
    public bool HasRole(UserId userId, string role)
    {
        var currentUser = currentUserProvider.GetCurrentUser();

        var havePermission = currentUser.Roles.Contains(role);

        return havePermission;
    }

    public bool HasPermission(UserId userId, string permissionName)
    {
        var currentUser = currentUserProvider.GetCurrentUser();

        var havePermission = currentUser.Permissions.Contains(permissionName);

        return havePermission;
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
        var isInRole = await context
            .UserRoles
            .FirstOrDefaultAsync(r => r.UserId == userId && r.RoleId == roleId, cancellationToken);

        if (isInRole is not null)
        {
            context.UserRoles.Remove(isInRole);
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
        var isInPermission = await context
            .UserPermissions
            .FirstOrDefaultAsync(r => r.UserId == userId && r.PermissionId == permissionId, cancellationToken);

        if (isInPermission is not null)
        {
            context.UserPermissions.Remove(isInPermission);
        }
    }

    private Task<Result<TokenResponse>> GenerateUserTokenAsync(User user)
    {
        throw new NotImplementedException();
    }
}
