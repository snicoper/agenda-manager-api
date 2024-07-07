using AgendaManager.Application.Common.Interfaces.Users;
using AgendaManager.Application.Common.Models.Users;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Infrastructure.Common.Authentication;

public class AuthorizationService(ICurrentUserProvider currentUserProvider)
    : IAuthorizationService
{
    public Task<Result<TokenResponse>> LoginAsync(string email, string password)
    {
        throw new NotImplementedException();
    }

    public Task<Result<TokenResponse>> RefreshTokenAsync(string refreshToken)
    {
        throw new NotImplementedException();
    }

    public bool HasRole(UserId userId, string role)
    {
        var currentUser = currentUserProvider.GetCurrentUser();

        var havePermission = currentUser.Roles.Contains(role);

        return havePermission;
    }

    public bool HasPermission(UserId userId, string permissionName)
    {
        var currentUser = currentUserProvider.GetCurrentUser();

        var havePermission = currentUser.Permissions.Contains(permissionName);

        return havePermission;
    }

    private Task<Result<TokenResponse>> GenerateUserTokenAsync(User user)
    {
        throw new NotImplementedException();
    }
}
