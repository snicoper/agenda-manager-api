using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Events;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users;

public sealed class User : AggregateRoot
{
    private readonly List<Role> _roles = [];

    internal User(
        UserId userId,
        EmailAddress email,
        string passwordHash,
        string? firstName,
        string? lastName,
        bool active = true,
        bool emailConfirmed = false)
    {
        Id = userId;
        Email = email;
        PasswordHash = passwordHash;
        FirstName = firstName;
        LastName = lastName;
        Active = active;
        IsEmailConfirmed = emailConfirmed;

        Active = true;
        IsEmailConfirmed = false;

        AddDomainEvent(new UserCreatedDomainEvent(userId));
    }

    private User()
    {
    }

    public UserId Id { get; } = null!;

    public string PasswordHash { get; private set; } = default!;

    public EmailAddress Email { get; private set; } = null!;

    public bool IsEmailConfirmed { get; private set; }

    public string? FirstName { get; private set; }

    public string? LastName { get; private set; }

    public bool Active { get; private set; }

    public RefreshToken? RefreshToken { get; private set; }

    public IReadOnlyCollection<Role> Roles => _roles.AsReadOnly();

    public void UpdateRefreshToken(RefreshToken refreshToken)
    {
        if (RefreshToken is not null && RefreshToken.Equals(refreshToken))
        {
            return;
        }

        RefreshToken = refreshToken;

        AddDomainEvent(new UserRefreshTokenUpdatedDomainEvent(Id));
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

    internal void UpdateUser(string? firstName, string? lastName)
    {
        if (FirstName == firstName && LastName == lastName)
        {
            return;
        }

        FirstName = firstName;
        LastName = lastName;

        AddDomainEvent(new UserUpdatedDomainEvent(Id));
    }

    internal Result AddRole(Role role)
    {
        _roles.Add(role);

        AddDomainEvent(new UserRoleAddedDomainEvent(Id, role.Id));

        return Result.Success();
    }

    internal Result RemoveRole(Role role)
    {
        _roles.Remove(role);

        AddDomainEvent(new UserRoleRemovedDomainEvent(Id, role.Id));

        return Result.Success();
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
