using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.Enums;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.TestCommon.Factories;

public class UserTokenFactory
{
    public static UserToken CreateUserToken(
        UserTokenId? userTokenId = null,
        UserId? userId = null,
        Token? token = null,
        UserTokenType? type = null)
    {
        var userToken = UserToken.Create(
            id: userTokenId ?? UserTokenId.Create(),
            userId: userId ?? UserId.Create(),
            token: token ?? Token.Generate(TimeSpan.FromMinutes(5)),
            type: type ?? UserTokenType.EmailConfirmation);

        return userToken;
    }
}
