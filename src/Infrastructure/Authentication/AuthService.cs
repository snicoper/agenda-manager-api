using AgendaManager.Application.Common.Interfaces.Users;
using AgendaManager.Application.Common.Models.Users;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users;

namespace AgendaManager.Infrastructure.Authentication;

public class AuthService : IAuthService
{
    public Task<Result<TokenResponse>> LoginAsync(string email, string password)
    {
        throw new NotImplementedException();
    }

    public Task<Result<TokenResponse>> RefreshTokenAsync(string refreshToken)
    {
        throw new NotImplementedException();
    }

    private Task<Result<TokenResponse>> GenerateUserTokenAsync(User user)
    {
        throw new NotImplementedException();
    }
}
