﻿using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Common.WeekDays;
using AgendaManager.Domain.ResourceManagement.Resources;
using AgendaManager.Domain.ResourceManagement.Resources.Entities;
using AgendaManager.Domain.ResourceManagement.Resources.Enums;
using AgendaManager.Domain.ResourceManagement.Resources.Interfaces;
using AgendaManager.Domain.ResourceManagement.Resources.ValueObjects;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.ValueObjects;
using AgendaManager.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Infrastructure.ResourceManagement.Resources.Persistence.Repositories;

public class ResourceRepository(AppDbContext context) : IResourceRepository
{
    public IQueryable<Resource> GetQueryable()
    {
        return context.Resources.AsQueryable();
    }

    public async Task<ICollection<ResourceSchedule>> GetSchedulesByResourceIdAsync(
        ResourceId resourceId,
        CalendarId calendarId,
        CancellationToken cancellationToken = default)
    {
        var schedules = await context.Resources
            .Where(r => r.Id == resourceId && r.CalendarId == calendarId)
            .SelectMany(r => r.Schedules)
            .ToListAsync(cancellationToken);

        return schedules;
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
        CalendarId calendarId,
        ResourceId resourceId,
        string name,
        CancellationToken cancellationToken = default)
    {
        var exists = await context.Resources.AnyAsync(
            r => r.Id != resourceId && r.CalendarId == calendarId && r.Name == name,
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

    public async Task AddAsync(Resource resource, CancellationToken cancellationToken = default)
    {
        await context.Resources.AddAsync(resource, cancellationToken);
    }

    public void Update(Resource resource)
    {
        context.Resources.Update(resource);
    }

    public void Delete(Resource resource)
    {
        context.Resources.Remove(resource);
    }
}
