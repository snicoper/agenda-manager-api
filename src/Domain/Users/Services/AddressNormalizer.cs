using System.Globalization;

namespace AgendaManager.Domain.Users.Services;

public static class AddressNormalizer
{
    public static string NormalizeStreet(string street)
    {
        return street.Trim();
    }

    public static string NormalizeCity(string city)
    {
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(city.Trim().ToLower());
    }

    public static string NormalizeState(string state)
    {
        return state.Trim().ToUpperInvariant();
    }

    public static string NormalizeCountry(string country)
    {
        return country.Trim().ToUpperInvariant();
    }

    public static string NormalizePostalCode(string postalCode)
    {
        return postalCode.Trim().Replace(" ", string.Empty);
    }
}
