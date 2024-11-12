using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects.Token;
using AgendaManager.Domain.Users.Enums;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Events;
using AgendaManager.Domain.Users.Exceptions;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Entities;

public sealed class UserToken : AuditableEntity
{
    private UserToken()
    {
    }

    private UserToken(UserTokenId id, UserId userId, Token token, UserTokenType type)
    {
        GuardAgainstInvalidType(type);

        Id = id;
        UserId = userId;
        Token = token;
        Type = type;
    }

    public UserTokenId Id { get; } = null!;

    public UserId UserId { get; private set; } = null!;

    public Token Token { get; } = null!;

    public UserTokenType Type { get; private set; } = UserTokenType.None;

    public bool IsExpired => Token.IsExpired;

    public static UserToken Create(UserTokenId id, UserId userId, Token token, UserTokenType type)
    {
        UserToken userToken = new(id, userId, token, type);

        userToken.AddDomainEvent(new UserTokenCreatedDomainEvent(id));

        return userToken;
    }

    public static Result<UserToken> CreateEmailConfirmation(UserId userId, TimeSpan? validityPeriod = null)
    {
        validityPeriod ??= TimeSpan.FromDays(7);

        var userToken = new UserToken(
            UserTokenId.Create(),
            userId,
            Token.Generate(validityPeriod.Value),
            UserTokenType.EmailConfirmation);

        return userToken;
    }

    public static Result<UserToken> CreatePasswordReset(UserId userId, TimeSpan? validityPeriod = null)
    {
        validityPeriod ??= TimeSpan.FromHours(1);

        var userToken = new UserToken(
            UserTokenId.Create(),
            userId,
            Token.Generate(validityPeriod.Value),
            UserTokenType.PasswordReset);

        return userToken;
    }

    public Result Consume(string tokenValue)
    {
        if (IsExpired)
        {
            return UserTokenErrors.TokenHasExpired;
        }

        if (!Token.Validate(tokenValue))
        {
            return UserTokenErrors.InvalidToken;
        }

        AddDomainEvent(new UserTokenConsumedDomainEvent(Id));

        return Result.Success();
    }

    private static void GuardAgainstInvalidType(UserTokenType type)
    {
        if (type == UserTokenType.None)
        {
            throw new UserTokenDomainException("User token type cannot be None.");
        }
    }
}
