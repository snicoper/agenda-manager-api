using AgendaManager.Domain.Users;

namespace AgendaManager.Application.Common.Abstractions.Users;

public interface IJwtTokenGenerator
{
    Task<string> GenerateAccessTokenAsync(User user);

    string GenerateRefreshToken();
}
