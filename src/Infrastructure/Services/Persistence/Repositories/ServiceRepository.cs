﻿using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.ValueObjects;
using AgendaManager.Domain.Services;
using AgendaManager.Domain.Services.Interfaces;
using AgendaManager.Domain.Services.ValueObjects;
using AgendaManager.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Infrastructure.Services.Persistence.Repositories;

public class ServiceRepository(AppDbContext context) : IServiceRepository
{
    public async Task<Service?> GetByIdAsync(ServiceId serviceId, CancellationToken cancellationToken = default)
    {
        var calendar = await context.Services.FirstOrDefaultAsync(s => s.Id == serviceId, cancellationToken);

        return calendar;
    }

    public async Task<Service?> GetByIdWithResourceTypesAsync(
        ServiceId serviceId,
        CancellationToken cancellationToken = default)
    {
        var calendar = await context.Services
            .Include(s => s.ResourceTypes)
            .FirstOrDefaultAsync(s => s.Id == serviceId, cancellationToken);

        return calendar;
    }

    public async Task<bool> AnyByResourceTypeIdAsync(
        ResourceTypeId resourceTypeId,
        CancellationToken cancellationToken = default)
    {
        var anyService = await context.Services.AnyAsync(
            s => s.ResourceTypes.Any(rt => rt.Id == resourceTypeId),
            cancellationToken);

        return anyService;
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var exists = await context.Services.AnyAsync(s => s.Name == name, cancellationToken);

        return exists;
    }

    public async Task<bool> HasServicesInCalendarAsync(
        CalendarId calendarId,
        CancellationToken cancellationToken = default)
    {
        var hasServices = await context.Services.AnyAsync(s => s.CalendarId == calendarId, cancellationToken);

        return hasServices;
    }

    public async Task AddAsync(Service service, CancellationToken cancellationToken = default)
    {
        await context.Services.AddAsync(service, cancellationToken);
    }

    public void Delete(Service service)
    {
        context.Services.Remove(service);
    }
}
