using System.Security.Claims;
using AgendaManager.Application.Common.Interfaces.Users;

namespace AgendaManager.WebApi.Services;

public class CurrentUserProvider(IHttpContextAccessor httpContextAccessor)
    : ICurrentUserProvider
{
    public Guid Id => Guid.Parse(
        httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Sid) ?? Guid.Empty.ToString());

    public IEnumerable<Guid> Roles =>
        httpContextAccessor
            .HttpContext?
            .User.FindAll(ClaimTypes.Role)
            .Select(r => Guid.Parse(r.Value)) ?? [];
}
