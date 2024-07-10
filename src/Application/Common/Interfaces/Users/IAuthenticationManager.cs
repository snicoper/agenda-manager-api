using AgendaManager.Application.Common.Models.Users;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Application.Common.Interfaces.Users;

public interface IAuthenticationManager
{
    Task<Result<TokenResult>> LoginAsync(string email, string password);

    Task<Result<TokenResult>> RefreshTokenAsync(string refreshToken);
}
