using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Interfaces;

public interface IUserRepository
{
    IQueryable<User> GetQueryable();

    Task<User?> GetByIdAsync(UserId userId, CancellationToken cancellationToken = default);

    Task<User?> GetByIdWithRolesAsync(UserId userId, CancellationToken cancellationToken = default);

    Task<User?> GetByIdWithRolesAndPermissionsAsync(UserId userId, CancellationToken cancellationToken = default);

    Task<User?> GetByEmailAsync(EmailAddress email, CancellationToken cancellationToken = default);

    Task AddAsync(User newUser, CancellationToken cancellationToken = default);

    void Update(User user);
}
