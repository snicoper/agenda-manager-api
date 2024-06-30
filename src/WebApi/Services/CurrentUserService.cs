using System.Security.Claims;
using AgendaManager.Application.Common.Abstractions.Users;

namespace AgendaManager.WebApi.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor)
    : ICurrentUserService
{
    public Guid Id => Guid.Parse(httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Sid) ?? Guid.Empty.ToString());

    public IEnumerable<Guid> Roles =>
        httpContextAccessor
            .HttpContext?
            .User.FindAll(ClaimTypes.Role)
            .Select(r => Guid.Parse(r.Value)) ?? [];
}
