using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Common.WekDays;
using AgendaManager.Domain.ResourceManagement.Resources;
using AgendaManager.Domain.ResourceManagement.Resources.Enums;
using AgendaManager.Domain.ResourceManagement.Resources.Interfaces;
using AgendaManager.Domain.ResourceManagement.Resources.ValueObjects;
using AgendaManager.Domain.ResourceManagement.ResourceTypes;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.ValueObjects;
using AgendaManager.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Infrastructure.ResourceManagement.Resources.Persistence.Repositories;

public class ResourceRepository(AppDbContext context) : IResourceRepository
{
    public IQueryable<ResourceType> GetQueryable()
    {
        return context.ResourceTypes.AsQueryable();
    }

    public async Task<Resource?> GetByIdAsync(ResourceId resourceId, CancellationToken cancellationToken = default)
    {
        var resource = await context.Resources.FirstOrDefaultAsync(r => r.Id == resourceId, cancellationToken);

        return resource;
    }

    public async Task<bool> AnyByTypeIdAsync(
        ResourceTypeId resourceTypeId,
        CancellationToken cancellationToken = default)
    {
        var any = await context.Resources.AnyAsync(r => r.TypeId == resourceTypeId, cancellationToken);

        return any;
    }

    public async Task<bool> ExistsByNameAsync(
        ResourceId resourceId,
        string name,
        CancellationToken cancellationToken = default)
    {
        var exists = await context.Resources.AnyAsync(r => r.Id != resourceId && r.Name == name, cancellationToken);

        return exists;
    }

    public async Task<bool> ExistsByDescriptionAsync(
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
            .Where(
                r => r.CalendarId == calendarId &&
                    resources.Select(res => res.Id).Contains(r.Id))
            .AnyAsync(
                r => r.CalendarId == calendarId &&

                    // Has regular schedule that applies?
                    r.Schedules.Any(
                        rs =>
                            rs.IsActive &&
                            rs.Type == ResourceScheduleType.Available &&
                            rs.Period.Start <= period.End &&
                            rs.Period.End >= period.Start &&
                            (rs.AvailableDays & (WeekDays)(1 << (int)period.Start.DayOfWeek)) != 0) &&

                    // No exceptions that invalidate it?
                    !r.Schedules.Any(
                        rs =>
                            rs.IsActive &&
                            rs.Type == ResourceScheduleType.Unavailable &&
                            rs.Period.Start <= period.End &&
                            rs.Period.End >= period.Start &&
                            (rs.AvailableDays & (WeekDays)(1 << (int)period.Start.DayOfWeek)) != 0),
                cancellationToken);

        return !resourcesAvailability;
    }

    public async Task<bool> HasResourcesInCalendarAsync(
        CalendarId calendarId,
        CancellationToken cancellationToken = default)
    {
        var hasResources = await context.Resources.AnyAsync(r => r.CalendarId == calendarId, cancellationToken);

        return hasResources;
    }
}
