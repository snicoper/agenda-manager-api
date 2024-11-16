using AgendaManager.Domain.Common.Exceptions;

namespace AgendaManager.Domain.Calendars.Exceptions;

public class CalendarConfigurationDomainException : DomainException
{
    public CalendarConfigurationDomainException(string message)
        : base(message)
    {
    }

    public CalendarConfigurationDomainException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
