using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Persistence;
using AgendaManager.Domain.Users.ValueObjects;
using AgendaManager.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Infrastructure.Users;

public class UserRepository(AppDbContext context) : IUsersRepository
{
    public IQueryable<User> GetAllQueryable()
    {
        return context.Users;
    }

    public async Task<User?> GetByIdAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);
    }

    public async Task AddAsync(User newUser, CancellationToken cancellationToken = default)
    {
        await context.Users.AddAsync(newUser, cancellationToken);
    }

    public void Update(User existingUser, User updatedUser)
    {
        context.Entry(existingUser).CurrentValues.SetValues(updatedUser);
    }

    public Task<bool> IsInRoleAsync(UserId userId, string role, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> AuthorizeAsync(UserId userId, string policyName, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
