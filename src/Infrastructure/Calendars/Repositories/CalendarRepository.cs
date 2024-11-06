using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Infrastructure.Calendars.Repositories;

public class CalendarRepository(AppDbContext context) : ICalendarRepository
{
    public async Task AddAsync(Calendar calendar, CancellationToken cancellationToken = default)
    {
        await context.Calendars.AddAsync(calendar, cancellationToken);
    }

    public void Update(Calendar calendar)
    {
        context.Calendars.Update(calendar);
    }

    public async Task<bool> NameIsUniqueAsync(Calendar calendar, CancellationToken cancellationToken = default)
    {
        return await context.Calendars
            .AnyAsync(c => c.Name.Equals(calendar.Name) && c.Id != calendar.Id, cancellationToken);
    }

    public async Task<bool> DescriptionIsUniqueAsync(Calendar calendar, CancellationToken cancellationToken = default)
    {
        return await context.Calendars
            .AnyAsync(c => c.Description.Equals(calendar.Description) && c.Id != calendar.Id, cancellationToken);
    }
}
