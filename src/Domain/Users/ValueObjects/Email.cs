using System.Text.RegularExpressions;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Users.Exceptions;

namespace AgendaManager.Domain.Users.ValueObjects;

public class Email : ValueObject
{
    private Email(string value)
    {
        Value = value;

        if (!Validate())
        {
            throw new InvalidEmailException();
        }
    }

    public string Value { get; }

    public static Email From(string value)
    {
        return new Email(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    private bool Validate()
    {
        var pattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
        var regex = new Regex(pattern);

        return regex.IsMatch(Value);
    }
}
