using System.ComponentModel.DataAnnotations.Schema;
using AgendaManager.Domain.Authorization;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Events;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users;

public sealed class User : AuditableEntity
{
    private string _passwordHash = default!;

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
        _passwordHash = passwordHash;
        FirstName = firstName;
        LastName = lastName;

        Active = true;
        IsEmailConfirmed = false;
    }

    public UserId Id { get; } = null!;

    public string UserName { get; private set; } = default!;

    public EmailAddress Email { get; private set; } = null!;

    public bool IsEmailConfirmed { get; private set; }

    public string? FirstName { get; private set; }

    public string? LastName { get; private set; }

    public bool Active { get; private set; }

    public RefreshToken? RefreshToken { get; private set; }

    public ICollection<UserRole> UserRoles { get; } = new HashSet<UserRole>();

    public ICollection<UserPermission> UserPermissions { get; } = new HashSet<UserPermission>();

    [NotMapped]
    public IReadOnlyCollection<Role> Roles => UserRoles.Select(ur => ur.Role).ToList();

    [NotMapped]
    public IReadOnlyCollection<Permission> Permissions => UserPermissions.Select(up => up.Permission).ToList();

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

    public User UpdateUser(string firstName, string lastName)
    {
        if (FirstName == firstName && LastName == lastName)
        {
            return this;
        }

        FirstName = firstName;
        LastName = lastName;

        AddDomainEvent(new UserUpdatedDomainEvent(Id));

        return this;
    }

    public bool VerifyPassword(string rawPassword, IPasswordHasher passwordHasher)
    {
        return passwordHasher.VerifyPassword(rawPassword, _passwordHash);
    }

    public Result UpdatePassword(string rawPassword, IPasswordHasher passwordHasher)
    {
        if (VerifyPassword(rawPassword, passwordHasher))
        {
            return Result.Success();
        }

        var passwordHashResult = passwordHasher.HashPassword(rawPassword);

        if (passwordHashResult.IsFailure || string.IsNullOrEmpty(passwordHashResult.Value))
        {
            return passwordHashResult;
        }

        _passwordHash = passwordHashResult.Value;

        AddDomainEvent(new UserPasswordUpdatedDomainEvent(Id));

        return Result.Success();
    }

    public User UpdateEmail(EmailAddress email)
    {
        if (Email.Equals(email))
        {
            return this;
        }

        Email = email;

        AddDomainEvent(new UserEmailUpdatedDomainEvent(Id));

        return this;
    }

    public User UpdateRefreshToken(RefreshToken refreshToken)
    {
        if (RefreshToken is not null && RefreshToken.Equals(refreshToken))
        {
            return this;
        }

        RefreshToken = refreshToken;

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

    public User SetActiveState(bool state)
    {
        if (Active == state)
        {
            return this;
        }

        Active = state;

        AddDomainEvent(new UserActiveStateChangedDomainEvent(Id, state));

        return this;
    }
}
