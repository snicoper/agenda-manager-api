﻿using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects.EmailAddress;
using AgendaManager.Domain.Common.ValueObjects.Token;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Events;
using AgendaManager.Domain.Users.Exceptions;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users;

public sealed class User : AggregateRoot
{
    private readonly List<Role> _roles = [];

    internal User(
        UserId userId,
        EmailAddress email,
        PasswordHash passwordHash,
        string? firstName,
        string? lastName,
        bool active = true,
        bool emailConfirmed = false)
    {
        GuardAgainstInvalidFirstName(firstName);
        GuardAgainstInvalidLastName(lastName);

        Id = userId;
        Email = email;
        PasswordHash = passwordHash;
        FirstName = firstName;
        LastName = lastName;
        Active = active;
        IsEmailConfirmed = emailConfirmed;

        AddDomainEvent(new UserCreatedDomainEvent(userId));
    }

    private User()
    {
    }

    public UserId Id { get; } = null!;

    public PasswordHash PasswordHash { get; private set; } = default!;

    public EmailAddress Email { get; private set; } = null!;

    public bool IsEmailConfirmed { get; private set; }

    public string? FirstName { get; private set; }

    public string? LastName { get; private set; }

    public bool Active { get; private set; }

    public Token? RefreshToken { get; private set; }

    public IReadOnlyCollection<Role> Roles => _roles.AsReadOnly();

    public Result UpdatePassword(PasswordHash newPasswordHash)
    {
        if (PasswordHash.Equals(newPasswordHash))
        {
            return Result.Success();
        }

        PasswordHash = newPasswordHash;

        AddDomainEvent(new UserPasswordUpdatedDomainEvent(Id));

        return Result.Success();
    }

    public async Task<Result> UpdateEmail(
        EmailAddress newEmail,
        IEmailUniquenessChecker emailUniquenessChecker,
        CancellationToken cancellationToken)
    {
        if (Email.Equals(newEmail))
        {
            return Result.Success();
        }

        if (!await emailUniquenessChecker.IsUnique(newEmail, cancellationToken))
        {
            return UserErrors.EmailAlreadyExists;
        }

        Email = newEmail;

        AddDomainEvent(new UserEmailUpdatedDomainEvent(Id));

        return Result.Success();
    }

    public void UpdateRefreshToken(Token token)
    {
        if (RefreshToken is not null && RefreshToken.Equals(token))
        {
            return;
        }

        RefreshToken = token;

        AddDomainEvent(new UserRefreshTokenUpdatedDomainEvent(Id));
    }

    public void SetEmailConfirmed()
    {
        if (IsEmailConfirmed)
        {
            return;
        }

        IsEmailConfirmed = true;

        AddDomainEvent(new UserEmailConfirmedDomainEvent(Id));
    }

    public void UpdateActiveState(bool newState)
    {
        if (Active == newState)
        {
            return;
        }

        Active = newState;

        AddDomainEvent(new UserActiveStateUpdatetedDomainEvent(Id, newState));
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

    private static void GuardAgainstInvalidFirstName(string? firstName)
    {
        if (!string.IsNullOrWhiteSpace(firstName) && firstName.Length > 256)
        {
            throw new UserDomainException("First name exceeds length of 256 characters.");
        }
    }

    private static void GuardAgainstInvalidLastName(string? lastName)
    {
        if (!string.IsNullOrWhiteSpace(lastName) && lastName.Length > 256)
        {
            throw new UserDomainException("Last name exceeds length of 256 characters.");
        }
    }
}
