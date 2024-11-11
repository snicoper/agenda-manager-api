using AgendaManager.Domain.Common.Exceptions;

namespace AgendaManager.Domain.Calendars.Exceptions;

public class CalendarDomainException(string message) : DomainException(message)
{
}
