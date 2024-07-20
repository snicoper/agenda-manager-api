using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Interfaces;

public interface IAuthorizationManager
{
    bool HasRole(UserId userId, string role);

    bool HasPermission(UserId userId, string permissionName);

    Task<List<Role>> GetRolesByUserIdAsync(UserId userId, CancellationToken cancellationToken = default);

    Task<List<User>> GetUsersByRoleIdAsync(RoleId roleId, CancellationToken cancellationToken = default);

    Task AddRoleAsync(UserId userId, RoleId roleId, CancellationToken cancellationToken = default);

    Task RemoveRoleAsync(UserId userId, RoleId roleId, CancellationToken cancellationToken = default);

    Task<List<Permission>> GetPermissionsByRoleId(RoleId roleId, CancellationToken cancellationToken = default);

    void AddPermissionToRoleAsync(Role role, Permission permission);
}
