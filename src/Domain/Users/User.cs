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

    public string UserName { get; init; } = default!;

    public EmailAddress Email { get; init; } = null!;

    public bool IsEmailConfirmed { get; init; }

    public string? FirstName { get; init; }

    public string? LastName { get; init; }

    public bool Active { get; init; }

    public string PasswordHash { get; init; } = default!;

    public RefreshToken? RefreshToken { get; init; }

    public static User Create(
        UserId userId,
        EmailAddress email,
        string userName,
        string passwordHash,
        string? firstName,
        string? lastName)
    {
        var user = new User(userId, email, userName, passwordHash, firstName, lastName);

        user.AddDomainEvent(new UserCreatedDomainEvent(userId));

        return user;
    }

    public User UpdatePassword(string passwordHash)
    {
        User userUpdated = new(Id, Email, UserName, passwordHash, FirstName, LastName)
        {
            Active = Active,
            IsEmailConfirmed = IsEmailConfirmed,
            RefreshToken = RefreshToken,
            Created = Created,
            CreatedBy = CreatedBy
        };

        AddDomainEvent(new UserPasswordUpdatedDomainEvent(Id));

        return userUpdated;
    }

    public User UpdateEmail(EmailAddress email)
    {
        User userUpdated = new(Id, email, UserName, PasswordHash, FirstName, LastName)
        {
            Active = Active,
            IsEmailConfirmed = IsEmailConfirmed,
            RefreshToken = RefreshToken,
            Created = Created,
            CreatedBy = CreatedBy
        };

        AddDomainEvent(new UserEmailUpdatedDomainEvent(Id));

        return userUpdated;
    }

    public User UpdateRefreshToken(RefreshToken refreshToken)
    {
        User userUpdated = new(Id, Email, UserName, PasswordHash, FirstName, LastName)
        {
            Active = Active,
            IsEmailConfirmed = IsEmailConfirmed,
            RefreshToken = refreshToken,
            Created = Created,
            CreatedBy = CreatedBy
        };

        return userUpdated;
    }

    public User Deactivate()
    {
        if (!Active)
        {
            return this;
        }

        User userUpdated = new(Id, Email, UserName, PasswordHash, FirstName, LastName)
        {
            Active = false,
            IsEmailConfirmed = IsEmailConfirmed,
            RefreshToken = RefreshToken,
            Created = Created,
            CreatedBy = CreatedBy
        };

        AddDomainEvent(new UserDeactivatedDomainEvent(Id));

        return userUpdated;
    }

    public User Activate()
    {
        if (Active)
        {
            return this;
        }

        User userUpdated = new(Id, Email, UserName, PasswordHash, FirstName, LastName)
        {
            Active = true,
            IsEmailConfirmed = IsEmailConfirmed,
            RefreshToken = RefreshToken,
            Created = Created,
            CreatedBy = CreatedBy
        };

        AddDomainEvent(new UserActivatedDomainEvent(Id));

        return userUpdated;
    }

    public User ConfirmEmail()
    {
        User userUpdated = new(Id, Email, UserName, PasswordHash, FirstName, LastName)
        {
            Active = Active,
            IsEmailConfirmed = true,
            RefreshToken = RefreshToken,
            Created = Created,
            CreatedBy = CreatedBy
        };

        AddDomainEvent(new UserEmailConfirmedDomainEvent(Id));

        return userUpdated;
    }
}
