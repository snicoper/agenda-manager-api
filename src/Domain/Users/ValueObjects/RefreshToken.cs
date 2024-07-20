﻿using System.Security.Cryptography;
using AgendaManager.Domain.Common.Abstractions;

namespace AgendaManager.Domain.Users.ValueObjects;

public class RefreshToken : ValueObject
{
    private const int TokenLength = 200;

    private RefreshToken(string token, DateTimeOffset expiryTime, Guid id)
    {
        Token = token;
        ExpiryTime = expiryTime;
        Id = id;
    }

    public string Token { get; }

    public DateTimeOffset ExpiryTime { get; }

    public Guid Id { get; }

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

        var expiryTime = DateTimeOffset.UtcNow.Add(lifetime);

        return new RefreshToken(token, expiryTime, Guid.NewGuid());
    }

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

        return new RefreshToken(token, expiryTime, Guid.NewGuid());
    }

    public bool IsExpired()
    {
        return DateTimeOffset.UtcNow >= ExpiryTime;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Token;
        yield return ExpiryTime;
        yield return Id;
    }
}
