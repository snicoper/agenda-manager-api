using AgendaManager.Application.Authentication.Models;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Application.Authentication.Interfaces;

public interface IJwtTokenGenerator
{
    Task<TokenResult> GenerateAccessTokenAsync(UserId userId, CancellationToken cancellationToken = default);
}
