using AgendaManager.Domain.Users.Enums;

namespace AgendaManager.Domain.Users.ValueObjects;

public sealed record IdentityDocument
{
    private IdentityDocument(string value, string countryCode, IdentityDocumentType type)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(countryCode);
        ArgumentNullException.ThrowIfNull(type);

        Value = value;
        CountryCode = countryCode;
        Type = type;
    }

    public string Value { get; }

    public string CountryCode { get; }

    public IdentityDocumentType Type { get; }

    public static IdentityDocument From(string value, string countryCode, IdentityDocumentType type)
    {
        return new IdentityDocument(value, countryCode, type);
    }
}
