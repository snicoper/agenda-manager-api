using AgendaManager.Application.Common.Interfaces.Users;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Infrastructure.Users;

public class AuthorizationChecker(ICurrentUserProvider currentUserProvider) : IAuthorizationChecker
{
    public bool HasRole(UserId userId, string role)
    {
        if (currentUserProvider.IsAuthenticated is false)
        {
            return false;
        }

        var currentUser = currentUserProvider.GetCurrentUser();
        var hasRole = currentUser?.Roles.Contains(role);

        return hasRole ?? false;
    }

    public bool HasPermission(UserId userId, string permissionName)
    {
        if (currentUserProvider.IsAuthenticated is false)
        {
            return false;
        }

        var currentUser = currentUserProvider.GetCurrentUser();
        var hasPermission = currentUser?.Permissions.Contains(permissionName);

        return hasPermission ?? false;
    }
}
