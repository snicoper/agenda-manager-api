using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Application.Authentication.Interfaces;

public interface IAuthorizationChecker
{
    bool HasRole(UserId userId, string role);

    bool HasPermission(UserId userId, string permissionName);
}
