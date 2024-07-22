using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Application.Common.Interfaces.Users;

public interface IAuthorizationChecker
{
    bool HasRole(UserId userId, string role);

    bool HasPermission(UserId userId, string permissionName);
}
