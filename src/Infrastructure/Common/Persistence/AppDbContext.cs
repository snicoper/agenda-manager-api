﻿using System.Reflection;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Infrastructure.Common.Persistence;

public class AppDbContext(
    DbContextOptions<AppDbContext> options,
    IHttpContextAccessor httpContextAccessor,
    IPublisher publisher)
    : DbContext(options), IUnitOfWork
{
    public DbSet<User> Users => Set<User>();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker
            .Entries<Entity>()
            .Select(entry => entry.Entity.DomainEvents)
            .SelectMany(events => events)
            .ToList();

        if (IsUserWaitingOnline())
        {
            AddDomainEventsToOfflineProcessingQueue(domainEvents);
        }
        else
        {
            foreach (var domainEvent in domainEvents)
            {
                await publisher.Publish(domainEvent, cancellationToken);
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

    private bool IsUserWaitingOnline()
    {
        return httpContextAccessor.HttpContext is not null;
    }

    private void AddDomainEventsToOfflineProcessingQueue(List<IDomainEvent> domainEvents)
    {
        var domainEventsQueue = httpContextAccessor.HttpContext!.Items
            .TryGetValue("DomainEventsQueue", out var value) && value is Queue<IDomainEvent> existingDomainEvents
            ? existingDomainEvents
            : new Queue<IDomainEvent>();

        domainEvents.ForEach(domainEventsQueue.Enqueue);

        httpContextAccessor.HttpContext!.Items["DomainEventsQueue"] = domainEventsQueue;
    }
}
