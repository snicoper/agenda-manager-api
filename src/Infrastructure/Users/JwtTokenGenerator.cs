using System.Security.Cryptography;
using AgendaManager.Application.Common.Interfaces.Users;
using AgendaManager.Domain.Users;

namespace AgendaManager.Infrastructure.Users;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    public Task<string> GenerateAccessTokenAsync(User user)
    {
        throw new NotImplementedException();
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(randomNumber);
        var tokenRefresh = Convert.ToBase64String(randomNumber);

        return tokenRefresh;
    }
}
