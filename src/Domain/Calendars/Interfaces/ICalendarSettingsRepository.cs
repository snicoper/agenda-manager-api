using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.ValueObjects;

namespace AgendaManager.Domain.Calendars.Interfaces;

public interface ICalendarSettingsRepository
{
    Task<CalendarSettings?> GetSettingsByCalendarIdAsync(
        CalendarId id,
        CancellationToken cancellationToken = default);
}
