using AgendaManager.Domain.Common.Exceptions;

namespace AgendaManager.Domain.Common.ValueObjects.Period;

public class InvalidPeriodException : DomainException
{
    public InvalidPeriodException(string message)
        : base(message)
    {
    }
}
