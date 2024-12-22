using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Infrastructure.Calendars.Persistence.Repositories;

public class CalendarRepository(AppDbContext context) : ICalendarRepository
{
    public IQueryable<Calendar> GetQueryable()
    {
        return context.Calendars.AsQueryable();
    }

    public async Task<Calendar?> GetByIdAsync(CalendarId id, CancellationToken cancellationToken = default)
    {
        var calendar = await context.Calendars.FirstOrDefaultAsync(c => c.Id.Equals(id), cancellationToken);

        return calendar;
    }

    public Task<bool> ExistsByCalendarIdAsync(CalendarId calendarId, CancellationToken cancellationToken = default)
    {
        var exists = context.Calendars.AnyAsync(c => c.Id.Equals(calendarId), cancellationToken);

        return exists;
    }

    public async Task<bool> ExistsByNameAsync(
        CalendarId calendarId,
        string name,
        CancellationToken cancellationToken = default)
    {
        var exists = await context.Calendars
            .AnyAsync(c => c.Id == calendarId && c.Name == name, cancellationToken);

        return exists;
    }

    public async Task AddAsync(Calendar calendar, CancellationToken cancellationToken = default)
    {
        await context.Calendars.AddAsync(calendar, cancellationToken);
    }

    public void Update(Calendar calendar)
    {
        context.Calendars.Update(calendar);
    }

    public void Delete(Calendar calendar)
    {
        context.Calendars.Remove(calendar);
    }
}
