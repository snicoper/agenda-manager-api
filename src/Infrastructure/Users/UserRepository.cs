using AgendaManager.Application.Common.Exceptions;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Persistence;
using AgendaManager.Domain.Users.ValueObjects;
using AgendaManager.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Infrastructure.Users;

public class UserRepository(AppDbContext context) : IUsersRepository
{
    public async Task<User?> GetByIdAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }

    public async Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        await context.Users.AddAsync(user, cancellationToken);

        return user;
    }

    public async Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        var existingUSer = await context.Users.FindAsync([user.Id, cancellationToken], cancellationToken);

        if (existingUSer is null)
        {
            throw new NotFoundException(nameof(User), nameof(UserId));
        }

        context.Entry(existingUSer).CurrentValues.SetValues(user);

        return user;
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
