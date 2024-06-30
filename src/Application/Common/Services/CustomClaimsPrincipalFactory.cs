using System.Security.Claims;
using AgendaManager.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace AgendaManager.Application.Common.Services;

public class CustomClaimsPrincipalFactory(UserManager<User> userManager, IOptions<IdentityOptions> optionsAccessor)
    : UserClaimsPrincipalFactory<User>(userManager, optionsAccessor)
{
    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
    {
        ClaimsIdentity identity = await base.GenerateClaimsAsync(user);

        // identity.AddClaim(new Claim(CustomClaims.CompanyId, user.CompanyId));
        return identity;
    }
}
