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

    internal static List<OutboxMessage> GenerateMessagesForTesting(IEnumerable<IEntity> entities)
    {
        return GenerateOutboxMessages(entities);
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

        var outboxMessages = GenerateOutboxMessages(entities);

        foreach (var entity in entities)
        {
            entity.ClearDomainEvents();
        }

        await context.Set<OutboxMessage>().AddRangeAsync(outboxMessages, cancellationToken);
    }

    private static List<OutboxMessage> GenerateOutboxMessages(IEnumerable<IEntity> entities)
    {
        var enumerable = entities as IEntity[] ?? entities.ToArray();

        var domainEvents = enumerable
            .SelectMany(e => e.DomainEvents)
            .ToList();

        if (domainEvents.Count == 0)
        {
            return [];
        }

        var messages = domainEvents.Select(
                domainEvent =>
                    OutboxMessage.Create(
                        OutboxMessageId.Create(),
                        domainEvent.GetType().Name,
                        JsonConvert.SerializeObject(domainEvent)))
            .ToList();

        foreach (var entity in enumerable)
        {
            entity.ClearDomainEvents();
        }

        return messages;
    }
}
