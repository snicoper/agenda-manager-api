using AgendaManager.Domain.Common.Exceptions;

namespace AgendaManager.Domain.Common.ValueObjects.Duration;

public class DurationException : DomainException
{
    public DurationException(string message)
        : base(message)
    {
    }

    public DurationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
