﻿using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Exceptions;
using AgendaManager.Domain.Users.Interfaces;

namespace AgendaManager.Domain.Users.ValueObjects;

public sealed record PasswordHash
{
    private PasswordHash(string hashedValue)
    {
        ArgumentNullException.ThrowIfNull(hashedValue);

        HashedValue = hashedValue;
    }

    public string HashedValue { get; }

    public static PasswordHash FromHashed(string hashedPassword)
    {
        if (string.IsNullOrWhiteSpace(hashedPassword))
        {
            throw new UserDomainException("Hashed password cannot be empty.");
        }

        return new PasswordHash(hashedPassword);
    }

    public static Result<PasswordHash> FromRaw(
        string rawPassword,
        IPasswordHasher passwordHasher,
        IPasswordPolicy passwordPolicy)
    {
        var validationResult = passwordPolicy.ValidatePassword(rawPassword);

        if (validationResult.IsFailure)
        {
            return validationResult.MapToValue<PasswordHash>();
        }

        var hashedPassword = passwordHasher.HashPassword(rawPassword);
        PasswordHash passwordHash = new(hashedPassword);

        return Result.Success(passwordHash);
    }

    public bool Verify(string rawPassword, IPasswordHasher passwordHasher)
    {
        return passwordHasher.VerifyPassword(rawPassword, this);
    }
}
