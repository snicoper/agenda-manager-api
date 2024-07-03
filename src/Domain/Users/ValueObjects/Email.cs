using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.Utils;
using AgendaManager.Domain.Users.Exceptions;

namespace AgendaManager.Domain.Users.ValueObjects;

public class Email : ValueObject
{
    private Email(string value)
    {
        if (!Validate())
        {
            throw new InvalidEmailException();
        }

        Value = value;
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
        var regex = DomainRegex.Email();

        return regex.IsMatch(Value);
    }
}
