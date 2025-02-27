﻿using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.ResourceManagement.Resources.Entities;
using AgendaManager.Domain.ResourceManagement.Resources.ValueObjects;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.ValueObjects;

namespace AgendaManager.Domain.ResourceManagement.Resources.Interfaces;

public interface IResourceRepository
{
    IQueryable<Resource> GetQueryable();

    Task<ICollection<ResourceSchedule>> GetSchedulesByResourceIdAsync(
        ResourceId resourceId,
        CalendarId calendarId,
        CancellationToken cancellationToken = default);

    Task<Resource?> GetByIdAsync(ResourceId resourceId, CancellationToken cancellationToken = default);

    Task<bool> AnyByTypeIdAsync(ResourceTypeId resourceTypeId, CancellationToken cancellationToken = default);

    Task<bool> ExistsByNameAsync(
        CalendarId calendarId,
        ResourceId resourceId,
        string name,
        CancellationToken cancellationToken = default);

    Task<bool> AreResourcesAvailableInPeriodAsync(
        CalendarId calendarId,
        List<Resource> resources,
        Period period,
        CancellationToken cancellationToken = default);

    Task<bool> HasResourcesInCalendarAsync(CalendarId calendarId, CancellationToken cancellationToken = default);

    Task AddAsync(Resource resource, CancellationToken cancellationToken = default);

    void Update(Resource resource);

    void Delete(Resource resource);
}
