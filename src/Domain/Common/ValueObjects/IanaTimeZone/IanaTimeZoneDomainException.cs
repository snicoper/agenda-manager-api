using AgendaManager.Domain.Common.Exceptions;

namespace AgendaManager.Domain.Common.ValueObjects.IanaTimeZone;

public class IanaTimeZoneDomainException : DomainException
{
    public IanaTimeZoneDomainException(string message)
        : base(message)
    {
    }

    public IanaTimeZoneDomainException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
