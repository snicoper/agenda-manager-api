using AgendaManager.Application.Common.Models.Users;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Application.Common.Interfaces.Users;

public interface IAuthenticationManager
{
    Task<Result<TokenResponse>> LoginAsync(string email, string password);

    Task<Result<TokenResponse>> RefreshTokenAsync(string refreshToken);
}
