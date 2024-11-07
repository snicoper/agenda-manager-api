using AgendaManager.Domain.Common.Abstractions;
using DomainRegex = AgendaManager.Domain.Common.Utils.DomainRegex;

namespace AgendaManager.Domain.Common.ValueObjects.EmailAddress;

public class EmailAddress : ValueObject
{
    private EmailAddress(string value)
    {
        Value = value;

        if (!Validate())
        {
            throw new InvalidEmailAddressException();
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
        return !(string.IsNullOrEmpty(Value) || Value.Length > 256 || DomainRegex.ValidEmail().IsMatch(Value) is false);
    }
}
