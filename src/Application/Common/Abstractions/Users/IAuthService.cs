using AgendaManager.Application.Common.Models.Users;
using AgendaManager.Domain.Common.Abstractions;

namespace AgendaManager.Application.Common.Abstractions.Users;

public interface IAuthService
{
    Task<Result<TokenResponse>> LoginAsync(string email, string password);

    Task<Result<TokenResponse>> RefreshTokenAsync(string refreshToken);
}
