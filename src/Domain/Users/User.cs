﻿using System.ComponentModel.DataAnnotations.Schema;
using AgendaManager.Domain.Authorization;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Users.Events;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users;

public class User : AuditableEntity
{
    private User()
    {
    }

    private User(
        UserId userId,
        EmailAddress email,
        string userName,
        string passwordHash,
        string? firstName,
        string? lastName)
    {
        Id = userId;
        Email = email;
        UserName = userName;
        PasswordHash = passwordHash;
        FirstName = firstName;
        LastName = lastName;

        Active = true;
        RefreshToken = RefreshToken.Create(string.Empty, DateTimeOffset.UtcNow.AddDays(-1));
        IsEmailConfirmed = false;
    }

    public UserId Id { get; init; } = null!;

    public string UserName { get; private set; } = default!;

    public EmailAddress Email { get; } = null!;

    public bool IsEmailConfirmed { get; private set; }

    public string? FirstName { get; private set; }

    public string? LastName { get; private set; }

    public bool Active { get; private set; }

    public string PasswordHash { get; private set; } = default!;

    public RefreshToken? RefreshToken { get; private set; }

    public virtual ICollection<UserRole> UserRoles { get; } = new HashSet<UserRole>();

    public virtual ICollection<UserPermission> UserPermissions { get; } = new HashSet<UserPermission>();

    [NotMapped]
    public IEnumerable<Role> Roles => UserRoles.Select(ur => ur.Role).ToList();

    [NotMapped]
    public IEnumerable<Permission> Permissions => UserPermissions.Select(up => up.Permission).ToList();

    public static User Create(
        UserId userId,
        EmailAddress email,
        string userName,
        string passwordHash,
        string? firstName,
        string? lastName)
    {
        User user = new(userId, email, userName, passwordHash, firstName, lastName);

        user.AddDomainEvent(new UserCreatedDomainEvent(userId));

        return user;
    }

    public User UpdatePasswordHash(string passwordHash)
    {
        if (PasswordHash == passwordHash)
        {
            return this;
        }

        PasswordHash = passwordHash;

        AddDomainEvent(new UserPasswordUpdatedDomainEvent(Id));

        return this;
    }

    public User UpdateEmail(EmailAddress email)
    {
        if (Email.Equals(email))
        {
            return this;
        }

        AddDomainEvent(new UserEmailUpdatedDomainEvent(Id));

        return this;
    }

    public User UpdateRefreshToken(RefreshToken refreshToken)
    {
        if (RefreshToken!.Equals(refreshToken))
        {
            return this;
        }

        RefreshToken = refreshToken;

        return this;
    }

    public User Deactivate()
    {
        if (!Active)
        {
            return this;
        }

        Active = false;

        AddDomainEvent(new UserDeactivatedDomainEvent(Id));

        return this;
    }

    public User Activate()
    {
        if (Active)
        {
            return this;
        }

        Active = true;

        AddDomainEvent(new UserActivatedDomainEvent(Id));

        return this;
    }

    public User ConfirmEmail()
    {
        if (IsEmailConfirmed)
        {
            return this;
        }

        IsEmailConfirmed = true;

        AddDomainEvent(new UserEmailConfirmedDomainEvent(Id));

        return this;
    }
}
