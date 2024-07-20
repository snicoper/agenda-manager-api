using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Interfaces;

public interface IAuthorizationChecker
{
    bool HasRole(UserId userId, string role);

    bool HasPermission(UserId userId, string permissionName);
}
