using System.ComponentModel.DataAnnotations.Schema;
using AgendaManager.Domain.Authorization;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Users.Events;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users;

public sealed class User : AuditableEntity
{
    private User()
    {
    }

    private User(
        UserId userId,
        EmailAddress email,
        string passwordHash,
        string? firstName,
        string? lastName)
    {
        Id = userId;
        Email = email;
        PasswordHash = passwordHash;
        FirstName = firstName;
        LastName = lastName;

        Active = true;
        IsEmailConfirmed = false;
    }

    public UserId Id { get; } = null!;

    public string PasswordHash { get; private set; } = default!;

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
        string passwordHash,
        string? firstName,
        string? lastName)
    {
        User user = new(userId, email, passwordHash, firstName, lastName);

        user.AddDomainEvent(new UserCreatedDomainEvent(userId));

        return user;
    }

    public void UpdateUser(string firstName, string lastName)
    {
        if (FirstName == firstName && LastName == lastName)
        {
            return;
        }

        FirstName = firstName;
        LastName = lastName;

        AddDomainEvent(new UserUpdatedDomainEvent(Id));
    }

    public void UpdateRefreshToken(RefreshToken refreshToken)
    {
        if (RefreshToken is not null && RefreshToken.Equals(refreshToken))
        {
            return;
        }

        RefreshToken = refreshToken;
    }

    public void ConfirmEmail()
    {
        if (IsEmailConfirmed)
        {
            return;
        }

        IsEmailConfirmed = true;

        AddDomainEvent(new UserEmailConfirmedDomainEvent(Id));
    }

    public void SetActiveState(bool state)
    {
        if (Active == state)
        {
            return;
        }

        Active = state;

        AddDomainEvent(new UserActiveStateChangedDomainEvent(Id, state));
    }

    internal void UpdatePassword(string passwordHash)
    {
        PasswordHash = passwordHash;

        AddDomainEvent(new UserPasswordUpdatedDomainEvent(Id));
    }

    internal void UpdateEmail(EmailAddress email)
    {
        if (Email.Equals(email))
        {
            return;
        }

        Email = email;

        AddDomainEvent(new UserEmailUpdatedDomainEvent(Id));
    }
}
