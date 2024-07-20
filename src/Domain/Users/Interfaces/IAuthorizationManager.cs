using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Interfaces;

public interface IAuthorizationManager
{
    bool HasRole(UserId userId, string role);

    bool HasPermission(UserId userId, string permissionName);

    Task<Result> AddRoleToUserAsync(UserId userId, RoleId roleId, CancellationToken cancellationToken = default);

    Task<Result> AddPermissionToRole(
        RoleId roleId,
        PermissionId permissionId,
        CancellationToken cancellationToken = default);
}
