using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Persistence;

public interface IUsersRepository
{
    IQueryable<User> GetQueryable();

    Task<User?> GetByIdAsync(UserId userId, CancellationToken cancellationToken = default);

    Task<User?> GetByEmailAsync(EmailAddress email, CancellationToken cancellationToken = default);

    Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);

    Task AddAsync(User newUser, CancellationToken cancellationToken = default);

    void Update(User user);
}
