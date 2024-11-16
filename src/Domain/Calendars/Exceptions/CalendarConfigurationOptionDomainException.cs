using AgendaManager.Domain.Common.Exceptions;

namespace AgendaManager.Domain.Calendars.Exceptions;

public class CalendarConfigurationOptionDomainException : DomainException
{
    public CalendarConfigurationOptionDomainException(string message)
        : base(message)
    {
    }

    public CalendarConfigurationOptionDomainException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
