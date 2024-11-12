using AgendaManager.Domain.Common.Exceptions;

namespace AgendaManager.Domain.Calendars.Exceptions;

public class CalendarSettingsException : DomainException
{
    public CalendarSettingsException(string message)
        : base(message)
    {
    }

    public CalendarSettingsException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
