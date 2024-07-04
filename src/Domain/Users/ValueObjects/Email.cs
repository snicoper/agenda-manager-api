using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.Constants;
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
        return DomainRegex.ValidEmail().IsMatch(Value);
    }
}
