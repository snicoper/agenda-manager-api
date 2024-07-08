using AgendaManager.Domain.Common.Abstractions;

namespace AgendaManager.Domain.Users.ValueObjects;

public class RefreshToken : ValueObject
{
    private RefreshToken(string token, DateTimeOffset expiryTime)
    {
        Token = token;
        ExpiryTime = expiryTime;
    }

    public string Token { get; }

    public DateTimeOffset ExpiryTime { get; }

    public static RefreshToken Create(string token, DateTimeOffset expiryTime)
    {
        return new RefreshToken(token, expiryTime);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Token;
        yield return ExpiryTime;
    }
}
