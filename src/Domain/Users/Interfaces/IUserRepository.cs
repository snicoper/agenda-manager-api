using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Interfaces;

public interface IUserRepository
{
    IQueryable<User> GetQueryable();

    IQueryable<User> GetQueryableUsersByRoleId(RoleId roleId);

    IQueryable<User> GetQueryableUsersNotInRoleId(RoleId roleId);

    Task<User?> GetByIdAsync(UserId userId, CancellationToken cancellationToken = default);

    Task<(User User, UserToken Token)?> GetUserWithSpecificTokenAsync(
        UserTokenId userTokenId,
        CancellationToken cancellationToken = default);

    Task<User?> GetByIdWithRolesAsync(UserId userId, CancellationToken cancellationToken = default);

    Task<User?> GetByIdWithTokensAsync(UserId userId, CancellationToken cancellationToken = default);

    Task<User?> GetByIdWithProfileAsync(UserId from, CancellationToken cancellationToken);

    Task<User?> GetByIdWithAllInfoAsync(UserId userId, CancellationToken cancellationToken = default);

    Task<User?> GetByEmailAsync(EmailAddress email, CancellationToken cancellationToken = default);

    Task<User?> GetByEmailWithTokensAsync(EmailAddress email, CancellationToken cancellationToken = default);

    Task<User?> GetByTokenValueWithTokensAsync(string tokenValue, CancellationToken cancellationToken = default);

    Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);

    Task<bool> ExistsEmailAsync(EmailAddress email, CancellationToken cancellationToken = default);

    Task<bool> HasAnyUserWithRole(RoleId roleId, CancellationToken cancellationToken = default);

    Task AddAsync(User newUser, CancellationToken cancellationToken = default);

    void Update(User user);
}
