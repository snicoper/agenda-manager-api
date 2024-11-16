using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Infrastructure.Calendars.Repositories;

public class CalendarConfigurationOptionRepository(AppDbContext context) : ICalendarConfigurationOptionRepository
{
    public async Task<List<CalendarConfigurationOption>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var options = await context.CalendarConfigurationOptions.ToListAsync(cancellationToken);

        return options;
    }
}
