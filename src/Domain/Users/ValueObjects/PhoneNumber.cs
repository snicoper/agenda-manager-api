using AgendaManager.Domain.Common.Exceptions;
using PhoneNumbers;

namespace AgendaManager.Domain.Users.ValueObjects;

public sealed record PhoneNumber
{
    private const int MaxCountryCodeLength = 4;
    private const int MaxNumberLength = 15;

    private PhoneNumber(string number, string countryCode)
    {
        GuardAgainstInvalidNumber(number);
        GuardAgainstInvalidCountryCode(countryCode);

        if (!IsValidPhoneNumber(number, countryCode))
        {
            throw new DomainException("Invalid phone number");
        }

        Number = number;
        CountryCode = countryCode;
    }

    public string Number { get; }

    public string CountryCode { get; } = default!;

    public static PhoneNumber From(string number, string countryCode)
    {
        return new PhoneNumber(number, countryCode);
    }

    public string ToFormattedString()
    {
        var phoneNumberUtil = PhoneNumberUtil.GetInstance();
        var parsedNumber = phoneNumberUtil.Parse($"{CountryCode}{Number}", null);
        return phoneNumberUtil.Format(parsedNumber, PhoneNumberFormat.INTERNATIONAL);
    }

    public static bool IsValidPhoneNumber(string number, string countryCode)
    {
        try
        {
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();
            var parsedNumber = phoneNumberUtil.Parse($"{countryCode}{number}", null);

            return phoneNumberUtil.IsValidNumber(parsedNumber);
        }
        catch (NumberParseException)
        {
            return false;
        }
    }

    public override string ToString()
    {
        return $"{CountryCode} {Number}";
    }

    private static void GuardAgainstInvalidNumber(string number)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(number);

        if (number.Length > MaxNumberLength)
        {
            throw new DomainException("Phone number too long");
        }

        if (!number.All(char.IsDigit))
        {
            throw new DomainException("Phone number must contain only digits");
        }

        if (number.Length > MaxNumberLength)
        {
            throw new DomainException("Phone number too long");
        }
    }

    private static void GuardAgainstInvalidCountryCode(string countryCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(countryCode);

        if (countryCode.Length > MaxCountryCodeLength)
        {
            throw new DomainException("Country code too long");
        }

        if (!countryCode.StartsWith('+'))
        {
            throw new DomainException("Country code must start with '+'");
        }

        if (!countryCode[1..].All(char.IsDigit))
        {
            throw new DomainException("Invalid country code format");
        }

        if (countryCode.Length > MaxCountryCodeLength)
        {
            throw new DomainException("Country code too long");
        }
    }
}
