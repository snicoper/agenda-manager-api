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
            return UserErrors.UserNotFound;
        }

        var role = await roleRepository.GetByIdAsync(roleId, cancellationToken);

        if (role is null)
        {
            return UserErrors.RoleNotFound;
        }

        if (user.Roles.Any(r => r.Id.Equals(roleId)))
        {
            return UserErrors.RoleAlreadyExists;
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
            return UserErrors.UserNotFound;
        }

        var role = await roleRepository.GetByIdAsync(roleId, cancellationToken);

        if (role is null)
        {
            return UserErrors.RoleNotFound;
        }

        if (!user.Roles.Any(r => r.Id.Equals(roleId)))
        {
            return UserErrors.UserDoesNotHaveRoleAssigned;
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
            return UserErrors.RoleNotFound;
        }

        var permission = await permissionRepository.GetByIdAsync(permissionId, cancellationToken);

        if (permission is null)
        {
            return UserErrors.PermissionNotFound;
        }

        if (role.Permissions.Any(r => r.Id.Equals(permissionId)))
        {
            return UserErrors.PermissionAlreadyExists;
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
            return UserErrors.RoleNotFound;
        }

        var permission = await permissionRepository.GetByIdAsync(permissionId, cancellationToken);

        if (permission is null)
        {
            return UserErrors.PermissionNotFound;
        }

        if (!role.Permissions.Any(r => r.Id.Equals(permissionId)))
        {
            return UserErrors.RoleDoesNotHavePermissionAssigned;
        }

        var result = role.RemovePermission(permission);

        return result;
    }
}
