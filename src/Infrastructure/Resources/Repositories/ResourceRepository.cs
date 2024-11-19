using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.ValueObjects.Period;
using AgendaManager.Domain.Resources;
using AgendaManager.Domain.Resources.Enums;
using AgendaManager.Domain.Resources.Interfaces;
using AgendaManager.Domain.Resources.ValueObjects;
using AgendaManager.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Infrastructure.Resources.Repositories;

public class ResourceRepository(AppDbContext context) : IResourceRepository
{
    public async Task<Resource?> GetByIdAsync(ResourceId resourceId, CancellationToken cancellationToken = default)
    {
        var resource = await context.Resources.FirstOrDefaultAsync(r => r.Id == resourceId, cancellationToken);

        return resource;
    }

    public async Task<bool> NameExistsAsync(
        ResourceId resourceId,
        string name,
        CancellationToken cancellationToken = default)
    {
        var exists = await context.Resources.AnyAsync(r => r.Id != resourceId && r.Name == name, cancellationToken);

        return exists;
    }

    public async Task<bool> DescriptionExistsAsync(
        ResourceId resourceId,
        string name,
        CancellationToken cancellationToken = default)
    {
        var exists = await context.Resources.AnyAsync(
            r => r.Id != resourceId && r.Description == name,
            cancellationToken);

        return exists;
    }

    public async Task<bool> AreResourcesAvailableInPeriodAsync(
        CalendarId calendarId,
        List<Resource> resources,
        Period period,
        CancellationToken cancellationToken = default)
    {
        var resourcesAvailability = await context.Resources
            .Include(r => r.Schedules)
            .Where(r => r.CalendarId == calendarId && resources.Select(res => res.Id).Contains(r.Id))
            .AnyAsync(
                r => r.CalendarId == calendarId
                     && r.Schedules.Any(
                         rs => rs.Period.Start <= period.End
                               && rs.Period.End >= period.Start
                               && rs.Type == ResourceScheduleType.Available)
                     && !r.Schedules.Any(
                         rs => rs.Period.Start <= period.End
                               && rs.Period.End >= period.Start
                               && rs.Type == ResourceScheduleType.Unavailable),
                cancellationToken);

        return !resourcesAvailability;
    }
}
