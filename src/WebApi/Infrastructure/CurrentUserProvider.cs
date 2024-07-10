using System.Security.Claims;
using AgendaManager.Application.Common.Interfaces.Users;
using AgendaManager.Application.Common.Models.Users;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.WebApi.Infrastructure;

public class CurrentUserProvider(IHttpContextAccessor httpContextAccessor)
    : ICurrentUserProvider
{
    public CurrentUser GetCurrentUser()
    {
        var userId = GetClaimValues("id")
            .Select(Guid.Parse)
            .FirstOrDefault();

        var permissions = GetClaimValues("permissions");
        var roles = GetClaimValues(ClaimTypes.Role);

        return new CurrentUser(UserId.From(userId), Permissions: permissions, Roles: roles);
    }

    private IReadOnlyList<string> GetClaimValues(string claimType)
    {
        if (httpContextAccessor.HttpContext is null)
        {
            return Array.Empty<string>();
        }

        return httpContextAccessor.HttpContext.User.Claims
            .Where(claim => claim.Type == claimType)
            .Select(claim => claim.Value)
            .ToList();
    }
}
