using System.Security.Cryptography;
using AgendaManager.Domain.Common.Abstractions;

namespace AgendaManager.Domain.Users.ValueObjects;

public class RefreshToken : ValueObject
{
    private const int TokenLength = 200;

    private RefreshToken(string token, DateTimeOffset expires)
    {
        Token = token;
        Expires = expires;
    }

    public string Token { get; }

    public DateTimeOffset Expires { get; }

    public static RefreshToken Generate(TimeSpan lifetime)
    {
        var randomNumber = new byte[TokenLength / 2];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        var token = Convert.ToBase64String(randomNumber);

        if (token.Length > TokenLength)
        {
            token = token[..TokenLength];
        }

        var espirationDate = DateTimeOffset.UtcNow.Add(lifetime);

        return From(token, espirationDate);
    }

    public static RefreshToken From(string token, DateTimeOffset expires)
    {
        if (string.IsNullOrEmpty(token) || token.Length > TokenLength)
        {
            throw new ArgumentException(
                $"Value cannot be null, whitespace or greater than {TokenLength} characters.",
                nameof(token));
        }

        if (expires <= DateTimeOffset.UtcNow)
        {
            throw new ArgumentException("Value cannot be in the past.", nameof(expires));
        }

        return new RefreshToken(token, expires);
    }

    public bool IsExpired()
    {
        return DateTimeOffset.UtcNow >= Expires;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Token;
        yield return Expires;
    }
}
