using AgendaManager.Domain.Common.Exceptions;
using AgendaManager.Domain.Users.Services;

namespace AgendaManager.Domain.Users.ValueObjects;

public sealed record Address
{
    private const int MinLength = 2;
    private const int MaxStreetLength = 100;
    private const int MaxCityLength = 50;
    private const int MaxStateLength = 50;
    private const int MaxCountryLength = 50;
    private const int MaxPostalCodeLength = 10;

    private Address(string street, string city, string state, string country, string postalCode)
    {
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
        GuardAgainstInvalidAddress(street, city, state, country, postalCode);

        street = AddressNormalizer.NormalizeStreet(street);
        city = AddressNormalizer.NormalizeCity(city);
        state = AddressNormalizer.NormalizeState(state);
        country = AddressNormalizer.NormalizeCountry(country);
        postalCode = AddressNormalizer.NormalizePostalCode(postalCode);

        return new Address(street, city, state, country, postalCode);
    }

    private static void GuardAgainstInvalidAddress(
        string street,
        string city,
        string state,
        string country,
        string postalCode)
    {
        ArgumentNullException.ThrowIfNull(street);
        ArgumentNullException.ThrowIfNull(city);
        ArgumentNullException.ThrowIfNull(state);
        ArgumentNullException.ThrowIfNull(country);
        ArgumentNullException.ThrowIfNull(postalCode);

        if (string.IsNullOrWhiteSpace(street) || street.Length < MinLength || street.Length > MaxStreetLength)
        {
            throw new DomainException($"Street must be between {MinLength} and {MaxStreetLength} characters");
        }

        if (string.IsNullOrWhiteSpace(city) || city.Length < MinLength || city.Length > MaxCityLength)
        {
            throw new DomainException($"City must be between {MinLength} and {MaxCityLength} characters");
        }

        if (string.IsNullOrWhiteSpace(state) || state.Length < MinLength || state.Length > MaxStateLength)
        {
            throw new DomainException($"State must be between {MinLength} and {MaxStateLength} characters");
        }

        if (string.IsNullOrWhiteSpace(country) || country.Length < MinLength || country.Length > MaxCountryLength)
        {
            throw new DomainException($"Country must be between {MinLength} and {MaxCountryLength} characters");
        }

        if (string.IsNullOrWhiteSpace(postalCode)
            || postalCode.Length < MinLength
            || postalCode.Length > MaxPostalCodeLength)
        {
            throw new DomainException($"Postal code must be between {MinLength} and {MaxPostalCodeLength} characters");
        }
    }
}
