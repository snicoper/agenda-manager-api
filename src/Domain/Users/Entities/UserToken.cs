using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.ValueObjects.Token;
using AgendaManager.Domain.Users.Enums;
using AgendaManager.Domain.Users.Events;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Users.Entities;

public class UserToken : AuditableEntity
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

    public Token Token { get; private set; } = null!;

    public UserTokenType Type { get; private set; } = UserTokenType.None;

    public static UserToken Create(UserTokenId id, UserId userId, Token token, UserTokenType type)
    {
        UserToken userToken = new(id, userId, token, type);

        userToken.AddDomainEvent(new UserTokenCreatedDomainEvent(id));

        return userToken;
    }
}
