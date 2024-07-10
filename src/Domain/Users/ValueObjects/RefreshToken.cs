using AgendaManager.Domain.Common.Abstractions;

namespace AgendaManager.Domain.Users.ValueObjects;

public class RefreshToken : ValueObject
{
    private const int TokenLength = 200;

    private RefreshToken(string token, DateTimeOffset expiryTime)
    {
        Token = token;
        ExpiryTime = expiryTime;
    }

    public string Token { get; }

    public DateTimeOffset ExpiryTime { get; }

    public static RefreshToken Create(string token, DateTimeOffset expiryTime)
    {
        if (string.IsNullOrWhiteSpace(token) || token.Length > TokenLength)
        {
            throw new ArgumentException(
                $"Value cannot be null, whitespace or greater than {TokenLength} characters.",
                nameof(token));
        }

        if (expiryTime <= DateTimeOffset.UtcNow)
        {
            throw new ArgumentException("Value cannot be in the past.", nameof(expiryTime));
        }

        return new RefreshToken(token, expiryTime);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Token;
        yield return ExpiryTime;
    }
}
