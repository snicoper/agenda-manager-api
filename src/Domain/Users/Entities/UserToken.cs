using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Users.Enums;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Entities;

public sealed class UserToken : AuditableEntity
{
    private UserToken()
    {
    }

    private UserToken(UserTokenId id, UserId userId, Token token, UserTokenType type)
    {
        Id = id;
        UserId = userId;
        Token = token;
        Type = type;
    }

    public UserTokenId Id { get; } = null!;

    public UserId UserId { get; private set; } = null!;

    public Token Token { get; } = null!;

    public UserTokenType Type { get; private set; }

    public bool IsExpired => Token.IsExpired;

    internal static UserToken Create(UserTokenId id, UserId userId, Token token, UserTokenType type)
    {
        UserToken userToken = new(id, userId, token, type);

        return userToken;
    }

    internal Result Consume(string tokenValue)
    {
        if (IsExpired)
        {
            return UserTokenErrors.TokenHasExpired;
        }

        return !Token.Validate(tokenValue) ? UserTokenErrors.InvalidToken : Result.Success();
    }
}
