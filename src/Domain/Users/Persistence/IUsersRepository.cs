namespace AgendaManager.Domain.Users.Persistence;

public interface IUsersRepository
{
    Task<User?> GetByIdAsync(Guid id);

    Task<bool> IsInRoleAsync(Guid userId, string role);

    Task<bool> AuthorizeAsync(Guid userId, string policyName);
}
