using AgendaManager.Domain.Common.Exceptions;

namespace AgendaManager.Domain.Calendars.Exxceptions;

public class CalendarDomainException : DomainException
{
    public CalendarDomainException(string message)
        : base(message)
    {
    }

    public CalendarDomainException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
