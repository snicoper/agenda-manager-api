using System.Text.RegularExpressions;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Users.Exceptions;

namespace AgendaManager.Domain.Users.ValueObjects;

public partial class Email : ValueObject
{
    private Email()
    {
    }

    public string Value { get; private set; } = default!;

    public Email From(string value)
    {
        IsValid(value);
        Value = value;

        return new Email();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    [GeneratedRegex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")]
    private static partial Regex EmailRegex();

    private static bool IsValid(string email)
    {
        var regex = EmailRegex();
        var isValidEmail = regex.IsMatch(email);

        if (!isValidEmail)
        {
            throw new InvalidEmailException(email);
        }

        return true;
    }
}
