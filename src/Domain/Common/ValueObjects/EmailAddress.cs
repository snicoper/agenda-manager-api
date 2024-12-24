using AgendaManager.Domain.Common.Exceptions;
using AgendaManager.Domain.Common.Interfaces;
using DomainRegex = AgendaManager.Domain.Common.Utils.DomainRegex;

namespace AgendaManager.Domain.Common.ValueObjects;

public sealed record EmailAddress : IValueObject
{
    private EmailAddress(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        Value = value.ToLower();

        if (!IsValid())
        {
            throw new DomainException("Email address is invalid.");
        }
    }

    public string Value { get; }

    public static explicit operator string(EmailAddress emailAddress)
    {
        return emailAddress.Value;
    }

    public static EmailAddress From(string value)
    {
        return new EmailAddress(value);
    }

    private bool IsValid()
    {
        return !(string.IsNullOrWhiteSpace(Value)
            || Value.Length > 256
            || DomainRegex.ValidEmail().IsMatch(Value) is false);
    }
}
