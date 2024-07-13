using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Authorization.Interfaces;

public interface IAuthorizationManager
{
    bool HasRole(UserId userId, string role);

    bool HasPermission(UserId userId, string permissionName);

    Task<List<Role>> GetRolesByUserIdAsync(UserId userId, CancellationToken cancellationToken = default);

    Task<List<Permission>> GetPermissionsByUserIdAsync(UserId userId, CancellationToken cancellationToken = default);

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
