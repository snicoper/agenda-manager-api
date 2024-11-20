using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Services;

public class AuthorizationService(
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IPermissionRepository permissionRepository)
{
    public async Task<Result> AddRoleToUserAsync(UserId userId, RoleId roleId, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdWithRolesAsync(userId, cancellationToken);

        if (user is null)
        {
            return UserErrors.UserNotFound;
        }

        var role = await roleRepository.GetByIdAsync(roleId, cancellationToken);

        if (role is null)
        {
            return RoleErrors.RoleNotFound;
        }

        var result = user.AddRole(role);

        return result;
    }

    public async Task<Result> RemoveRoleFromUserAsync(UserId userId, RoleId roleId, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdWithRolesAsync(userId, cancellationToken);

        if (user is null)
        {
            return UserErrors.UserNotFound;
        }

        var role = await roleRepository.GetByIdAsync(roleId, cancellationToken);

        if (role is null)
        {
            return RoleErrors.RoleNotFound;
        }

        var result = user.RemoveRole(role);

        return result;
    }

    public async Task<Result> AddPermissionToRoleAsync(
        RoleId roleId,
        PermissionId permissionId,
        CancellationToken cancellationToken)
    {
        var role = await roleRepository.GetByIdAsync(roleId, cancellationToken);

        if (role is null)
        {
            return RoleErrors.RoleNotFound;
        }

        var permission = await permissionRepository.GetByIdAsync(permissionId, cancellationToken);

        if (permission is null)
        {
            return PermissionErrors.PermissionNotFound;
        }

        var result = role.AddPermission(permission);

        return result;
    }

    public async Task<Result> RemovePermissionFromRoleAsync(
        RoleId roleId,
        PermissionId permissionId,
        CancellationToken cancellationToken)
    {
        var role = await roleRepository.GetByIdAsync(roleId, cancellationToken);

        if (role is null)
        {
            return RoleErrors.RoleNotFound;
        }

        var permission = await permissionRepository.GetByIdAsync(permissionId, cancellationToken);

        if (permission is null)
        {
            return PermissionErrors.PermissionNotFound;
        }

        var result = role.RemovePermission(permission);

        return result;
    }
}
