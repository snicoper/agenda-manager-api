using System.Security.Claims;
using AgendaManager.Application.Common.Interfaces.Users;
using AgendaManager.Application.Common.Models.Users;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.WebApi.Services;

public class CurrentUserProvider(IHttpContextAccessor httpContextAccessor)
    : ICurrentUserProvider
{
    public CurrentUser GetCurrentUser()
    {
        var userId = GetClaimValues("id")
            .Select(param => UserId.From(Guid.Parse(param)))
            .First();

        var permissions = GetClaimValues("permissions");
        var roles = GetClaimValues(ClaimTypes.Role);

        return new CurrentUser(userId, Permissions: permissions, Roles: roles);
    }

    private IReadOnlyList<string> GetClaimValues(string claimType)
    {
        return httpContextAccessor.HttpContext!.User.Claims
            .Where(claim => claim.Type == claimType)
            .Select(claim => claim.Value)
            .ToList();
    }
}
