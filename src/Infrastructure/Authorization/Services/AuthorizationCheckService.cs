using AgendaManager.Application.Authorization.Interfaces;
using AgendaManager.Application.Users.Services;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Infrastructure.Authorization.Services;

public class AuthorizationCheckService(ICurrentUserProvider currentUserProvider)
    : IAuthorizationCheckService
{
    public bool HasPermissionOrIsOwner(string permission, UserId ownerId)
    {
        var currentUser = currentUserProvider.GetCurrentUser();

        if (currentUser is null)
        {
            return false;
        }

        var hasPermission = currentUser.UserId == ownerId || currentUser.Permissions.Contains(permission);

        return hasPermission;
    }
}
