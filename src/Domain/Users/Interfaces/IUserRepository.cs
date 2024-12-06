﻿using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Interfaces;

public interface IUserRepository
{
    IQueryable<User> GetQueryable();

    IQueryable<User> GetQueryableUsersByRoleId(RoleId roleId);

    IQueryable<User> GetQueryableUsersNotInRoleId(RoleId roleId);

    Task<User?> GetByIdAsync(UserId userId, CancellationToken cancellationToken = default);

    Task<User?> GetByIdWithRolesAsync(UserId userId, CancellationToken cancellationToken = default);

    Task<User?> GetByEmailAsync(EmailAddress email, CancellationToken cancellationToken = default);

    Task<User?> GetByEmailWithTokensAsync(EmailAddress email, CancellationToken cancellationToken = default);

    Task<User?> GetByTokenValueWithTokensAsync(string tokenValue, CancellationToken cancellationToken = default);

    Task<bool> EmailExistsAsync(EmailAddress email, CancellationToken cancellationToken = default);

    Task<bool> HasAnyUserWithRole(RoleId roleId, CancellationToken cancellationToken = default);

    Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);

    Task AddAsync(User newUser, CancellationToken cancellationToken = default);

    void Update(User user);
}
