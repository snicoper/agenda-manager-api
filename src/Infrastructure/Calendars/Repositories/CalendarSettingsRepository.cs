using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Infrastructure.Calendars.Repositories;

public class CalendarSettingsRepository(AppDbContext context) : ICalendarSettingsRepository
{
    public Task<CalendarSettings?> GetSettingsByCalendarIdAsync(
        CalendarId id,
        CancellationToken cancellationToken = default)
    {
        var settings = context.CalendarSettings.FirstOrDefaultAsync(cs => cs.CalendarId == id, cancellationToken);

        return settings;
    }
}
