namespace AgendaManager.Domain.Users.ValueObjects;

public sealed record PhoneNumber
{
    private PhoneNumber(string value, string countryCode)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentNullException.ThrowIfNull(countryCode);

        Value = value;
        CountryCode = countryCode;
    }

    public string Value { get; }

    public string CountryCode { get; } = default!;

    public static PhoneNumber From(string value, string countryCode)
    {
        return new PhoneNumber(value, countryCode);
    }
}
