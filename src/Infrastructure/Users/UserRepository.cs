using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Persistence;
using AgendaManager.Infrastructure.Common.Persistence;

namespace AgendaManager.Infrastructure.Users;

public class UserRepository(AppDbContext context) : IUsersRepository
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

    public async Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        await context.Users.AddAsync(user, cancellationToken);

        return user;
    }
}
