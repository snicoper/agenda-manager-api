using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Infrastructure.Common.Messaging.Interfaces;
using MediatR;
using Newtonsoft.Json;

namespace AgendaManager.Infrastructure.Common.Messaging.Services;

public class IntegrationEventDispatcher(IMediator mediator) : IIntegrationEventDispatcher
{
    private static readonly Dictionary<string, Type> DomainEventTypeMap = typeof(IDomainEvent)
        .Assembly
        .GetTypes()
        .Where(
            type => typeof(INotification).IsAssignableFrom(type) && type is { IsAbstract: false, IsInterface: false })
        .ToDictionary(type => type.Name, t => t);

    public async Task DispatchAsync(string routingKey, string payload, CancellationToken cancellationToken = default)
    {
        if (!DomainEventTypeMap.TryGetValue(routingKey, out var eventType))
        {
            throw new InvalidOperationException($"Unknown event type: {routingKey}");
        }

        var domainEvent = JsonConvert.DeserializeObject(payload, eventType)
            ?? throw new InvalidOperationException("Failed to deserialize the event.");

        await mediator.Publish((IDomainEvent)domainEvent, cancellationToken);
    }
}
