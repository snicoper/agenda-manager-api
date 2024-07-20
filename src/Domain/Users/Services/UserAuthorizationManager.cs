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

        return role is null ? IdentityUserErrors.RoleNotFound : user.AddRole(role);
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

        return permission is null ? IdentityUserErrors.PermissionNotFound : role.AddPermission(permission);
    }
}
