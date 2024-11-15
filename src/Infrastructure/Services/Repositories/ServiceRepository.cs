﻿using AgendaManager.Domain.Services;
using AgendaManager.Domain.Services.Interfaces;
using AgendaManager.Domain.Services.ValueObjects;
using AgendaManager.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Infrastructure.Services.Repositories;

public class ServiceRepository(AppDbContext context) : IServiceRepository
{
    public async Task<Service?> GetByIdAsync(ServiceId serviceId, CancellationToken cancellationToken = default)
    {
        var calendar = await context.Services.FirstOrDefaultAsync(s => s.Id == serviceId, cancellationToken);

        return calendar;
    }

    public async Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken = default)
    {
        var exists = await context.Services.AnyAsync(s => s.Name == name, cancellationToken);

        return exists;
    }

    public void Delete(Service service)
    {
        context.Services.Remove(service);
    }
}
