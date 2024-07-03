using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Persistence;

namespace AgendaManager.Infrastructure.Users.Persistence;

public class UserRepository : IUsersRepository
{
    public Task<User?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsInRoleAsync(Guid userId, string role)
    {
        throw new NotImplementedException();
    }

    public Task<bool> AuthorizeAsync(Guid userId, string policyName)
    {
        throw new NotImplementedException();
    }
}
