using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Authorization.Persistence;

public interface IAuthorizationManager
{
    bool HasRole(UserId userId, string role);

    bool HasPermission(UserId userId, string permissionName);
}
