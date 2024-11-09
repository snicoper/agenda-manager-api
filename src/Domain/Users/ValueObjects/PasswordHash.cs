using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Exceptions;
using AgendaManager.Domain.Users.Interfaces;

namespace AgendaManager.Domain.Users.ValueObjects;

public class PasswordHash : ValueObject
{
    private PasswordHash(string hashedValue)
    {
        HashedValue = hashedValue;
    }

    public string HashedValue { get; }

    public static explicit operator string(PasswordHash passwordHash)
    {
        return passwordHash.HashedValue;
    }

    public static PasswordHash FromHashed(string hashedPassword)
    {
        if (string.IsNullOrWhiteSpace(hashedPassword))
        {
            throw new UserDomainException("Hashed password cannot be empty");
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
        var password = new PasswordHash(hashedPassword);

        return Result.Success(password);
    }

    public bool Verify(string rawPassword, IPasswordHasher passwordHasher)
    {
        return passwordHasher.VerifyPassword(rawPassword, this);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return HashedValue;
    }
}
