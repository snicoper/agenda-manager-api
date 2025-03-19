using System.Text.Json;
using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.Common.Messaging;
using AgendaManager.Domain.Common.Messaging.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;

namespace AgendaManager.Infrastructure.Common.Persistence.Interceptors;

public class PersistDomainEventsToOutbox : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();

        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        await DispatchDomainEvents(eventData.Context, cancellationToken);

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static async Task DispatchDomainEvents(DbContext? context, CancellationToken cancellationToken = default)
    {
        if (context is null)
        {
            return;
        }

        var entities = context.ChangeTracker
            .Entries<IEntity>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToArray();

        var domainEvents = entities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        foreach (var entity in entities)
        {
            entity.ClearDomainEvents();
        }

        await SaveOutboxMessageDomainEvents(context, domainEvents, cancellationToken);
    }

    private static async Task SaveOutboxMessageDomainEvents(
        DbContext context,
        List<IDomainEvent> domainEvents,
        CancellationToken cancellationToken)
    {
        if (domainEvents.Count == 0)
        {
            return;
        }

        var outboxMessages = domainEvents.Select(
            domainEvent =>
                OutboxMessage.Create(
                    outboxMessageId: OutboxMessageId.Create(),
                    type: domainEvent.GetType().Name,
                    payload: JsonConvert.SerializeObject(domainEvent)));

        await context.Set<OutboxMessage>().AddRangeAsync(outboxMessages, cancellationToken);
    }
}
