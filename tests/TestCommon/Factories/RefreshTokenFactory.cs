using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.TestCommon.Factories;

public abstract class RefreshTokenFactory
{
    public const int TokenLength = 200;

    public static RefreshToken CreateValidRefreshToken()
    {
        var token = Guid.NewGuid().ToString();
        var expiryTime = DateTimeOffset.UtcNow.AddDays(1);

        return RefreshToken.Create(token, expiryTime);
    }

    public static RefreshToken CreateExpiredRefreshTime()
    {
        var token = Guid.NewGuid().ToString();
        var expiryTime = DateTimeOffset.MinValue;

        return RefreshToken.Create(token, expiryTime);
    }

    public static RefreshToken CreateInvalidToken()
    {
        var token = new string('a', TokenLength + 1);
        var expiryTime = DateTimeOffset.UtcNow.AddDays(1);

        return RefreshToken.Create(token, expiryTime);
    }

    public static RefreshToken CreateEmptyToken()
    {
        var token = string.Empty;
        var expiryTime = DateTimeOffset.UtcNow.AddDays(1);

        return RefreshToken.Create(token, expiryTime);
    }
}
