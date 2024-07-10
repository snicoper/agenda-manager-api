using AgendaManager.Application.Common.Models.Users;
using AgendaManager.Domain.Users;

namespace AgendaManager.Application.Common.Interfaces.Users;

public interface IJwtTokenGenerator
{
    Task<TokenResult> GenerateAccessTokenAsync(User user);
}
