using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Persistence;
using AgendaManager.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Infrastructure.Users;

public class UserRepository(AppDbContext context) : IUsersRepository
{
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Id.Value == id, cancellationToken);
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
