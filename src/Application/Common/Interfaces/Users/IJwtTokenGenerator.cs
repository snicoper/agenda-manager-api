using AgendaManager.Application.Common.Models.Users;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Application.Common.Interfaces.Users;

public interface IJwtTokenGenerator
{
    Task<TokenResult> GenerateAccessTokenAsync(UserId userId, CancellationToken cancellationToken = default);
}
