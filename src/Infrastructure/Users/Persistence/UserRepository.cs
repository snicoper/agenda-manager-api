using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AgendaManager.Infrastructure.Users.Persistence;

public class UserRepository(
    UserManager<User> userManager,
    IUserClaimsPrincipalFactory<User> userClaimsPrincipalFactory,
    IAuthorizationService authorizationService)
    : IUsersRepository
{
    public async Task<User?> GetByIdAsync(Guid id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());

        return user;
    }

    public async Task<bool> IsInRoleAsync(Guid userId, string role)
    {
        var user = await GetByIdAsync(userId);
        var result = user != null && await userManager.IsInRoleAsync(user, role);

        return result;
    }

    public async Task<bool> AuthorizeAsync(Guid userId, string policyName)
    {
        var user = await GetByIdAsync(userId);

        if (user is null)
        {
            return false;
        }

        var principal = await userClaimsPrincipalFactory.CreateAsync(user);
        var result = await authorizationService.AuthorizeAsync(principal, policyName);

        return result.Succeeded;
    }
}
