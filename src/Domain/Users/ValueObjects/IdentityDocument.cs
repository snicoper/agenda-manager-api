using AgendaManager.Domain.Common.Exceptions;
using AgendaManager.Domain.Users.Enums;

namespace AgendaManager.Domain.Users.ValueObjects;

public sealed record IdentityDocument
{
    private const int MaxValueLength = 20;
    private const int CountryCodeLength = 2;

    private IdentityDocument(string value, string countryCode, IdentityDocumentType type)
    {
        Value = value;
        CountryCode = countryCode;
        Type = type;
    }

    public string Value { get; private set; }

    public string CountryCode { get; private set; }

    public IdentityDocumentType Type { get; private set; }

    public static IdentityDocument From(string value, string countryCode, IdentityDocumentType type)
    {
        GuardAgainstInvalidIdentityDocument(value, countryCode, type);

        return new IdentityDocument(value, countryCode, type);
    }

    private static void GuardAgainstInvalidIdentityDocument(string value, string countryCode, IdentityDocumentType type)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(countryCode);
        ArgumentNullException.ThrowIfNull(type);

        if (string.IsNullOrWhiteSpace(value) || value.Length > MaxValueLength)
        {
            throw new DomainException($"ID value must not be empty and no longer than {MaxValueLength} characters");
        }

        if (countryCode.Length != CountryCodeLength)
        {
            throw new DomainException("Country code must be 2 characters (ISO format)");
        }

        if (!countryCode.All(char.IsLetter))
        {
            throw new DomainException("Country code must contain only letters");
        }
    }
}
