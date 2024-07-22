using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Services;

public class UserAuthorizationManager(
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IPermissionRepository permissionRepository)
{
    public async Task<Result> AddRoleToUserAsync(
        UserId userId,
        RoleId roleId,
        CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdWithRolesAsync(userId, cancellationToken);

        if (user is null)
        {
            return IdentityUserErrors.UserNotFound;
        }

        var role = await roleRepository.GetByIdAsync(roleId, cancellationToken);

        if (role is null)
        {
            return IdentityUserErrors.RoleNotFound;
        }

        if (user.Roles.Any(r => r.Id.Equals(roleId)))
        {
            return IdentityUserErrors.RoleAlreadyExists;
        }

        var result = user.AddRole(role);

        return result;
    }

    public async Task<Result> RemoveRoleFromUserAsync(
        UserId userId,
        RoleId roleId,
        CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdWithRolesAsync(userId, cancellationToken);

        if (user is null)
        {
            return IdentityUserErrors.UserNotFound;
        }

        var role = await roleRepository.GetByIdAsync(roleId, cancellationToken);

        if (role is null)
        {
            return IdentityUserErrors.RoleNotFound;
        }

        if (!user.Roles.Any(r => r.Id.Equals(roleId)))
        {
            return IdentityUserErrors.UserDoesNotHaveRoleAssigned;
        }

        var result = user.RemoveRole(role);

        return result;
    }

    public async Task<Result> AddPermissionToRole(
        RoleId roleId,
        PermissionId permissionId,
        CancellationToken cancellationToken = default)
    {
        var role = await roleRepository.GetByIdAsync(roleId, cancellationToken);

        if (role is null)
        {
            return IdentityUserErrors.RoleNotFound;
        }

        var permission = await permissionRepository.GetByIdAsync(permissionId, cancellationToken);

        if (permission is null)
        {
            return IdentityUserErrors.PermissionNotFound;
        }

        if (role.Permissions.Any(r => r.Id.Equals(permissionId)))
        {
            return IdentityUserErrors.PermissionAlreadyExists;
        }

        var result = role.AddPermission(permission);

        return result;
    }

    public async Task<Result> RemovePermissionFromRole(
        RoleId roleId,
        PermissionId permissionId,
        CancellationToken cancellationToken = default)
    {
        var role = await roleRepository.GetByIdAsync(roleId, cancellationToken);

        if (role is null)
        {
            return IdentityUserErrors.RoleNotFound;
        }

        var permission = await permissionRepository.GetByIdAsync(permissionId, cancellationToken);

        if (permission is null)
        {
            return IdentityUserErrors.PermissionNotFound;
        }

        if (!role.Permissions.Any(r => r.Id.Equals(permissionId)))
        {
            return IdentityUserErrors.RoleDoesNotHavePermissionAssigned;
        }

        var result = role.RemovePermission(permission);

        return result;
    }
}
