using AgendaManager.Domain.Common.ValueObjects.EmailAddress;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;
using AgendaManager.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Infrastructure.Users.Persistence.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public IQueryable<User> GetQueryable()
    {
        return context.Users.AsQueryable();
    }

    public async Task<User?> GetByIdAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }

    public async Task<User?> GetByIdWithRolesAsync(
        UserId userId,
        CancellationToken cancellationToken = default)
    {
        return await context
            .Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }

    public async Task<User?> GetByIdWithRolesAndPermissionsAsync(
        UserId userId,
        CancellationToken cancellationToken = default)
    {
        var user = await context
            .Users
            .Include(u => u.Roles)
            .ThenInclude(r => r.Permissions)
            .FirstOrDefaultAsync(u => u.Id.Equals(userId), cancellationToken);

        return user;
    }

    public async Task<User?> GetByEmailAsync(
        EmailAddress email,
        CancellationToken cancellationToken = default)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(User user, CancellationToken cancellationToken = default)
    {
        var emailExists = await context
            .Users
            .AnyAsync(u => u.Email == user.Email && u.Id != user.Id, cancellationToken);

        return emailExists;
    }

    public async Task<User?> GetByRefreshTokenAsync(
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        return await context.Users.FirstOrDefaultAsync(
            u => u.RefreshToken != null && u.RefreshToken.Token == refreshToken,
            cancellationToken);
    }

    public async Task AddAsync(User newUser, CancellationToken cancellationToken = default)
    {
        await context.Users.AddAsync(newUser, cancellationToken);
    }

    public void Update(User user)
    {
        context.Users.Update(user);
    }
}
