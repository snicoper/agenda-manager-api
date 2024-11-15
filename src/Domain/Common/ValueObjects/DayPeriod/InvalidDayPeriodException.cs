using AgendaManager.Domain.Common.Exceptions;

namespace AgendaManager.Domain.Common.ValueObjects.DayPeriod;

public class InvalidDayPeriodException : DomainException
{
    public InvalidDayPeriodException(string message)
        : base(message)
    {
    }
}
