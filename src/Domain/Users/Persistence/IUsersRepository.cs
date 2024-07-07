using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Persistence;

public interface IUsersRepository
{
    IQueryable<User> GetAllQueryable();

    Task<User?> GetByIdAsync(UserId userId, CancellationToken cancellationToken = default);

    Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);

    Task<User?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default);

    Task<bool> IsInRoleAsync(UserId userId, string role, CancellationToken cancellationToken = default);

    Task<bool> AuthorizeAsync(UserId userId, string permissionName, CancellationToken cancellationToken = default);

    Task AddAsync(User newUser, CancellationToken cancellationToken = default);

    void Update(User existingUser, User updatedUser);
}
