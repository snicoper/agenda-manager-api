using AgendaManager.Application.Common.Models.Users;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Application.Common.Interfaces.Users;

public interface IAuthorizationService
{
    Task<Result<TokenResponse>> LoginAsync(string email, string password);

    Task<Result<TokenResponse>> RefreshTokenAsync(string refreshToken);

    bool HasRole(UserId userId, string role);

    bool HasPermission(UserId userId, string permissionName);
}
