using AgendaManager.Domain.Users;

namespace AgendaManager.Application.Common.Interfaces.Users;

public interface IJwtTokenGenerator
{
    Task<string> GenerateAccessTokenAsync(User user);

    string GenerateRefreshToken();
}
