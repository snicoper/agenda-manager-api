using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.ValueObjects;

namespace AgendaManager.Domain.Calendars.Interfaces;

public interface ICalendarConfigurationRepository
{
    Task<List<CalendarConfiguration>> GetConfigurationsByCalendarIdAsync(
        CalendarId calendarId,
        CancellationToken cancellationToken = default);

    Task<CalendarConfiguration?> GetBySelectedKeyAsync(
        CalendarId calendarId,
        string selectedKey,
        CancellationToken cancellationToken = default);
}
