using AgendaManager.Domain.Users.Services;

namespace AgendaManager.Domain.Users.ValueObjects;

public sealed record Address
{
    private Address(string street, string city, string state, string country, string postalCode)
    {
        ArgumentNullException.ThrowIfNull(street);
        ArgumentNullException.ThrowIfNull(city);
        ArgumentNullException.ThrowIfNull(state);
        ArgumentNullException.ThrowIfNull(country);
        ArgumentNullException.ThrowIfNull(postalCode);

        Street = street;
        City = city;
        State = state;
        Country = country;
        PostalCode = postalCode;
    }

    public string Street { get; }

    public string City { get; }

    public string State { get; }

    public string Country { get; }

    public string PostalCode { get; }

    public static Address From(string street, string city, string state, string country, string postalCode)
    {
        street = AddressNormalizer.NormalizeStreet(street);
        city = AddressNormalizer.NormalizeCity(city);
        state = AddressNormalizer.NormalizeState(state);
        country = AddressNormalizer.NormalizeCountry(country);
        postalCode = AddressNormalizer.NormalizePostalCode(postalCode);

        return new Address(street, city, state, country, postalCode);
    }
}
