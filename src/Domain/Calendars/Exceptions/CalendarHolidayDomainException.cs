using AgendaManager.Domain.Common.Exceptions;

namespace AgendaManager.Domain.Calendars.Exceptions;

public class CalendarHolidayDomainException : DomainException
{
    public CalendarHolidayDomainException(string message)
        : base(message)
    {
    }

    public CalendarHolidayDomainException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
