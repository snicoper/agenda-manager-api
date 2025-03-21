﻿using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;
using AgendaManager.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Infrastructure.Users.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public IQueryable<User> GetQueryable()
    {
        return context.Users.AsQueryable();
    }

    public IQueryable<User> GetQueryableUsersByRoleId(RoleId roleId)
    {
        var users = context.Users
            .Include(u => u.UserRoles)
            .Where(u => u.UserRoles.Any(ur => ur.RoleId == roleId));

        return users;
    }

    public IQueryable<User> GetQueryableUsersNotInRoleId(RoleId roleId)
    {
        var users = context.Users
            .Include(u => u.UserRoles)
            .Where(u => u.UserRoles.All(ur => ur.RoleId != roleId));

        return users;
    }

    public async Task<User?> GetByIdAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        return user;
    }

    public async Task<(User User, UserToken Token)?> GetUserWithSpecificTokenAsync(
        UserTokenId userTokenId,
        CancellationToken cancellationToken = default)
    {
        var result = await context.Users
            .Where(u => u.Tokens.Any(t => t.Id == userTokenId))
            .Select(u => new { User = u, Token = u.Tokens.First(t => t.Id == userTokenId) })
            .FirstOrDefaultAsync(cancellationToken);

        return result is null ? null : (result.User, result.Token);
    }

    public async Task<User?> GetByIdWithRolesAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        var user = await context.Users
            .Include(u => u.UserRoles)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        return user;
    }

    public async Task<User?> GetByIdWithTokensAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        var user = await context.Users
            .Include(u => u.Tokens)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        return user;
    }

    public Task<User?> GetByIdWithProfileAsync(UserId from, CancellationToken cancellationToken)
    {
        var user = context.Users
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Id == from, cancellationToken);

        return user;
    }

    public Task<User?> GetByIdWithAllInfoAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        var user = context.Users
            .Include(u => u.UserRoles)
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        return user;
    }

    public async Task<User?> GetByEmailAsync(EmailAddress email, CancellationToken cancellationToken = default)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<User?> GetByEmailWithTokensAsync(
        EmailAddress email,
        CancellationToken cancellationToken = default)
    {
        var user = await context.Users
            .Include(u => u.Tokens)
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        return user;
    }

    public async Task<User?> GetByTokenValueWithTokensAsync(
        string tokenValue,
        CancellationToken cancellationToken = default)
    {
        var user = await context.Users
            .Include(u => u.Tokens)
            .Where(u => u.Tokens.Any(ut => ut.Token.Value == tokenValue))
            .FirstOrDefaultAsync(cancellationToken);

        return user;
    }

    public async Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var user = await context.Users.FirstOrDefaultAsync(
            u => u.RefreshToken != null && u.RefreshToken.Value == refreshToken,
            cancellationToken);

        return user;
    }

    public async Task<bool> ExistsEmailAsync(EmailAddress email, CancellationToken cancellationToken = default)
    {
        var emailExists = await context.Users.AnyAsync(u => u.Email == email, cancellationToken);

        return emailExists;
    }

    public async Task<bool> HasAnyUserWithRole(RoleId roleId, CancellationToken cancellationToken = default)
    {
        var hasAnyUserWithRole = await context.UserRoles
            .AnyAsync(ur => ur.RoleId == roleId, cancellationToken);

        return hasAnyUserWithRole;
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
