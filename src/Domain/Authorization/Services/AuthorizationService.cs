using AgendaManager.Domain.Authorization.Errors;
using AgendaManager.Domain.Authorization.Interfaces;
using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Authorization.Services;

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

        var userRole = UserRole.Create(user.Id, role.Id);
        var result = user.AddRole(userRole);

        userRepository.Update(user);

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

        var userRole = user.UserRoles.FirstOrDefault(ur => ur.RoleId == roleId);

        if (userRole is null)
        {
            return UserErrors.UserRoleNotFound;
        }

        var result = user.RemoveRole(userRole);

        userRepository.Update(user);

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
        roleRepository.Update(role);

        return result;
    }

    public async Task<Result> RemovePermissionFromRoleAsync(
        RoleId roleId,
        PermissionId permissionId,
        CancellationToken cancellationToken)
    {
        var role = await roleRepository.GetByIdWithPermissionsAsync(roleId, cancellationToken);

        if (role is null)
        {
            return RoleErrors.RoleNotFound;
        }

        var hasPermission = role.HasPermission(permissionId);

        if (!hasPermission)
        {
            return PermissionErrors.PermissionNotFound;
        }

        var permission = role.Permissions.FirstOrDefault(p => p.Id == permissionId);

        var result = role.RemovePermission(permission!);
        roleRepository.Update(role);

        return result;
    }
}
