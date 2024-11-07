using AgendaManager.Domain.Common.Exceptions;

namespace AgendaManager.Domain.Calendars.Exxceptions;

public class CalendarDomainException(string message) : DomainException(message)
{
}
