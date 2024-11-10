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

    public async Task<bool> NameExistsAsync(Calendar calendar, CancellationToken cancellationToken = default)
    {
        var nameIsUnique = await context
            .Calendars
            .AnyAsync(c => c.Name.Equals(calendar.Name) && c.Id != calendar.Id, cancellationToken);

        return nameIsUnique;
    }

    public async Task<bool> DescriptionExistsAsync(Calendar calendar, CancellationToken cancellationToken = default)
    {
        var descriptionIsUnique = await context.Calendars
            .AnyAsync(c => c.Description.Equals(calendar.Description) && c.Id != calendar.Id, cancellationToken);

        return descriptionIsUnique;
    }
}
