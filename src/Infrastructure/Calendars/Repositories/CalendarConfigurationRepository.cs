using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Infrastructure.Calendars.Repositories;

public class CalendarConfigurationRepository(AppDbContext context) : ICalendarConfigurationRepository
{
    public async Task<List<CalendarConfiguration>> GetConfigurationsByCalendarIdAsync(
        CalendarId calendarId,
        CancellationToken cancellationToken = default)
    {
        var configurations = await context.CalendarConfigurations
            .Where(cc => cc.CalendarId == calendarId)
            .ToListAsync(cancellationToken);

        return configurations;
    }

    public async Task<CalendarConfiguration?> GetBySelectedKeyAsync(
        CalendarId calendarId,
        string selectedKey,
        CancellationToken cancellationToken = default)
    {
        var configuration = await context.CalendarConfigurations
            .FirstOrDefaultAsync(
                cc => cc.CalendarId == calendarId
                      && cc.SelectedKey == selectedKey,
                cancellationToken);

        return configuration;
    }
}
