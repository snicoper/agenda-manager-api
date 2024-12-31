using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.Enums;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Events;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users;

public sealed class User : AggregateRoot
{
    private readonly List<UserRole> _userRoles = [];
    private readonly List<UserToken> _userTokens = [];

    internal User(
        UserId userId,
        EmailAddress email,
        PasswordHash passwordHash,
        bool emailConfirmed = false,
        bool isActive = true)
    {
        Id = userId;
        Email = email;
        PasswordHash = passwordHash;
        IsEmailConfirmed = emailConfirmed;
        IsActive = isActive;

        AddDomainEvent(new UserCreatedDomainEvent(userId));
    }

    private User()
    {
    }

    public UserId Id { get; } = null!;

    public UserProfileId ProfileId { get; private set; } = null!;

    public UserProfile Profile { get; private set; } = null!;

    public PasswordHash PasswordHash { get; private set; } = null!;

    public EmailAddress Email { get; private set; } = null!;

    public bool IsEmailConfirmed { get; private set; }

    public bool IsActive { get; private set; }

    public Token? RefreshToken { get; private set; }

    public IReadOnlyList<UserRole> UserRoles => _userRoles.AsReadOnly();

    public IReadOnlyList<UserToken> Tokens => _userTokens.AsReadOnly();

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

    public bool HasUserToken(UserTokenId userTokenId)
    {
        return _userTokens.Any(x => x.Id == userTokenId);
    }

    public Result<UserToken> CreateUserToken(UserTokenType type)
    {
        var validityPeriod = type switch
        {
            UserTokenType.EmailConfirmation => TimeSpan.FromMinutes(10),
            UserTokenType.PasswordReset => TimeSpan.FromDays(7),
            UserTokenType.AdminCreatedAccount => TimeSpan.FromDays(7),
            _ => throw new InvalidOperationException()
        };

        var userToken = UserToken.Create(
            id: UserTokenId.Create(),
            userId: Id,
            token: Token.Generate(validityPeriod),
            type: type);

        AddUserToken(userToken);

        userToken.AddDomainEvent(new UserTokenCreatedDomainEvent(userToken));

        return Result.Create(userToken);
    }

    public Result ConsumeUserToken(UserTokenId userTokenId, string tokenValue)
    {
        var userToken = _userTokens.FirstOrDefault(x => x.Id == userTokenId);

        if (userToken is null)
        {
            return UserTokenErrors.UserTokenNotFound;
        }

        RemoveUserToken(userToken);

        userToken.AddDomainEvent(new UserTokenConsumedDomainEvent(userToken.Id));

        return userToken.Consume(tokenValue);
    }

    public void AddUserToken(UserToken userToken)
    {
        if (HasUserToken(userToken.Id))
        {
            return;
        }

        _userTokens.Add(userToken);

        userToken.AddDomainEvent(new UserTokenAddedDomainEvent(Id, userToken.Id));
    }

    public void RemoveUserToken(UserToken userToken)
    {
        if (!HasUserToken(userToken.Id))
        {
            return;
        }

        _userTokens.Remove(userToken);

        userToken.AddDomainEvent(new UserTokenRemovedDomainEvent(Id, userToken.Id));
    }

    public bool HasRole(UserRole userRole)
    {
        var exists = _userRoles.Any(ur => ur.RoleId == userRole.RoleId);

        return exists;
    }

    internal Result AddRole(UserRole userRole)
    {
        if (HasRole(userRole))
        {
            return UserErrors.RoleAlreadyExists;
        }

        _userRoles.Add(userRole);
        AddDomainEvent(new UserRoleAddedDomainEvent(userRole));

        return Result.Success();
    }

    internal Result RemoveRole(UserRole userRole)
    {
        if (!HasRole(userRole))
        {
            return UserErrors.RoleDoesNotExist;
        }

        _userRoles.Remove(userRole);
        AddDomainEvent(new UserRoleRemovedDomainEvent(userRole));

        return Result.Success();
    }

    internal Result UpdatePassword(PasswordHash newPasswordHash)
    {
        if (PasswordHash == newPasswordHash)
        {
            return Result.Success();
        }

        PasswordHash = newPasswordHash;

        AddDomainEvent(new UserPasswordUpdatedDomainEvent(Id));

        return Result.Success();
    }

    internal void AddProfile(UserProfile profile)
    {
        ProfileId = profile.Id;
        Profile = profile;

        AddDomainEvent(new UserProfileAddedDomainEvent(profile.Id));
    }

    internal void UpdateProfile(
        string firstName,
        string lastName,
        PhoneNumber? phoneNumber = null,
        Address? address = null,
        IdentityDocument? identityDocument = null)
    {
        if (!Profile.HasChanges(firstName, lastName, phoneNumber, address, identityDocument))
        {
            return;
        }

        Profile.Update(firstName, lastName, phoneNumber, address, identityDocument);

        AddDomainEvent(new UserProfileUpdatedDomainEvent(Id));
    }
}
