using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Persistence;

public interface IUsersRepository
{
    Task<User?> GetByIdAsync(UserId userId, CancellationToken cancellationToken = default);

    Task<bool> IsInRoleAsync(UserId userId, string role, CancellationToken cancellationToken = default);

    Task<bool> AuthorizeAsync(UserId userId, string policyName, CancellationToken cancellationToken = default);

    Task<User> CreateAsync(User user, CancellationToken cancellationToken = default);

    Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default);
}
