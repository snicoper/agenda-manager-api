using System.Security.Claims;
using AgendaManager.Application.Common.Interfaces.Users;
using AgendaManager.Application.Common.Models.Users;
using AgendaManager.Domain.Common.Constants;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.WebApi.Infrastructure;

public class CurrentUserProvider(IHttpContextAccessor httpContextAccessor)
    : ICurrentUserProvider
{
    public bool IsAuthenticated => httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

    public CurrentUser? GetCurrentUser()
    {
        if (IsAuthenticated is false)
        {
            return null;
        }

        var userId = GetClaimValues(CustomClaimType.Id)
            .Select(Guid.Parse)
            .First();

        var permissions = GetClaimValues(CustomClaimType.Permissions);
        var roles = GetClaimValues(ClaimTypes.Role);

        return new CurrentUser(UserId.From(userId), roles, permissions);
    }

    private IReadOnlyList<string> GetClaimValues(string claimType)
    {
        var claim = httpContextAccessor.HttpContext!.User.Claims
            .Where(claim => claim.Type == claimType)
            .Select(claim => claim.Value)
            .ToList()
            .AsReadOnly();

        return claim;
    }
}
