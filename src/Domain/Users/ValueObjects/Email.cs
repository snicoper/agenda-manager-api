using System.Text.RegularExpressions;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Users.Exceptions;

namespace AgendaManager.Domain.Users.ValueObjects;

public partial class Email : ValueObject
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
        return EmailRegex().IsMatch(Value);
    }

    [GeneratedRegex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", RegexOptions.Compiled)]
    private partial Regex EmailRegex();
}
