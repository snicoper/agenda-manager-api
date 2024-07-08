using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Users.Exceptions;
using DomainRegex = AgendaManager.Domain.Common.Utils.DomainRegex;

namespace AgendaManager.Domain.Users.ValueObjects;

public class EmailAddress : ValueObject
{
    private EmailAddress(string value)
    {
        Value = value;

        if (!Validate())
        {
            throw new InvalidEmailException();
        }
    }

    public string Value { get; }

    public static EmailAddress From(string value)
    {
        return new EmailAddress(value);
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
