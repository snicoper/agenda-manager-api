using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Application.Common.Interfaces.Users;

public interface IAuthorizationManager
{
    bool HasRole(UserId userId, string role);

    bool HasPermission(UserId userId, string permissionName);
}
