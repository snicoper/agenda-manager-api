using AgendaManager.Domain.Common.Exceptions;
using DomainRegex = AgendaManager.Domain.Common.Utils.DomainRegex;

namespace AgendaManager.Domain.Common.ValueObjects;

public sealed record EmailAddress
{
    private EmailAddress(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        Value = value;

        if (!IsValid())
        {
            throw new DomainException("Email address is invalid.");
        }
    }

    public string Value { get; }

    public static EmailAddress From(string value)
    {
        return new EmailAddress(value);
    }

    private bool IsValid()
    {
        return !(string.IsNullOrWhiteSpace(Value) || Value.Length > 256 ||
                 DomainRegex.ValidEmail().IsMatch(Value) is false);
    }
}
