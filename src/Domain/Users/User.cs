using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects.EmailAddress;
using AgendaManager.Domain.Common.ValueObjects.Token;
using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Events;
using AgendaManager.Domain.Users.Exceptions;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users;

public sealed class User : AggregateRoot
{
    private readonly List<Role> _roles = [];
    private readonly List<UserToken> _userTokens = [];

    internal User(
        UserId userId,
        EmailAddress email,
        PasswordHash passwordHash,
        string? firstName,
        string? lastName,
        bool isActive = true,
        bool emailConfirmed = false)
    {
        GuardAgainstInvalidFirstName(firstName);
        GuardAgainstInvalidLastName(lastName);

        Id = userId;
        Email = email;
        PasswordHash = passwordHash;
        FirstName = firstName;
        LastName = lastName;
        IsActive = isActive;
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

    public bool IsActive { get; private set; }

    public Token? RefreshToken { get; private set; }

    public IReadOnlyCollection<Role> Roles => _roles.AsReadOnly();

    public IReadOnlyCollection<UserToken> Tokens => _userTokens.AsReadOnly();

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
        IEmailUniquenessPolicy emailUniquenessPolicy,
        CancellationToken cancellationToken)
    {
        if (Email == newEmail)
        {
            return Result.Success();
        }

        if (!await emailUniquenessPolicy.IsUnique(newEmail, cancellationToken))
        {
            return UserErrors.EmailAlreadyExists;
        }

        Email = newEmail;

        AddDomainEvent(new UserEmailUpdatedDomainEvent(Id));

        return Result.Success();
    }

    public void UpdateRefreshToken(Token token)
    {
        if (RefreshToken is not null && RefreshToken == token)
        {
            return;
        }

        RefreshToken = token;

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

    public void Activate()
    {
        if (IsActive)
        {
            return;
        }

        IsActive = true;
        AddDomainEvent(new UserActivatedDomainEvent(Id));
    }

    public void Deactivate()
    {
        if (!IsActive)
        {
            return;
        }

        IsActive = false;

        AddDomainEvent(new UserDeactivatedDomainEvent(Id));
    }

    public void AddUserToken(UserToken userToken)
    {
        _userTokens.Add(userToken);

        AddDomainEvent(new UserTokenAddedDomainEvent(Id, userToken.Id));
    }

    public void RemoveUserToken(UserToken userToken)
    {
        _userTokens.Remove(userToken);

        AddDomainEvent(new UserTokenRemovedDomainEvent(Id, userToken.Id));
    }

    public bool HasRole(RoleId roleId)
    {
        return _roles.Any(x => x.Id == roleId);
    }

    public bool HasPermission(PermissionId permissionId)
    {
        return _roles.Any(role => role.HasPermission(permissionId));
    }

    public IReadOnlyList<Permission> GetAllPermissions()
    {
        return _roles.SelectMany(x => x.Permissions)
            .ToList()
            .AsReadOnly();
    }

    internal void Update(string? firstName, string? lastName)
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
        if (HasRole(role.Id))
        {
            return Result.Success();
        }

        _roles.Add(role);
        AddDomainEvent(new UserRoleAddedDomainEvent(Id, role.Id));

        return Result.Success();
    }

    internal Result RemoveRole(Role role)
    {
        if (!HasRole(role.Id))
        {
            return Result.Success();
        }

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
