﻿using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.Utils;

namespace AgendaManager.Domain.Common.ValueObjects.Token;

public class Token : ValueObject
{
    private const int TokenLength = 200;

    private Token(string value, DateTimeOffset expires)
    {
        Value = value;
        Expires = expires;
    }

    public string Value { get; }

    public DateTimeOffset Expires { get; }

    public static Token Generate(TimeSpan lifetime)
    {
        var token = SecureTokenFactory.GenerateToken(TokenLength);
        var expires = DateTimeOffset.UtcNow.Add(lifetime);

        return From(token, expires);
    }

    public static Token From(string token, DateTimeOffset expires)
    {
        GuardAgainstToken(token);

        if (expires <= DateTimeOffset.UtcNow)
        {
            throw new ArgumentException("Value cannot be in the past.", nameof(expires));
        }

        return new Token(token, expires);
    }

    public bool IsExpired()
    {
        return DateTimeOffset.UtcNow >= Expires;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
        yield return Expires;
    }

    private static void GuardAgainstToken(string token)
    {
        if (string.IsNullOrEmpty(token) || token.Length > TokenLength)
        {
            throw new ArgumentException(
                $"Value cannot be null, whitespace or greater than {TokenLength} characters.",
                nameof(token));
        }
    }
}
