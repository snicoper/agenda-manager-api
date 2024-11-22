using AgendaManager.Domain.Calendars.ValueObjects;

namespace AgendaManager.Domain.Calendars.Interfaces;

public interface ICalendarNameValidationPolicy
{
    Task<bool> ExistsAsync(CalendarId calendarId, string name, CancellationToken cancellationToken = default);
}
