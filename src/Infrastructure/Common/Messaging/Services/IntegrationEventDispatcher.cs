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
        .Where(type => typeof(INotification).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface)
        .ToDictionary(type => type.Name, t => t);

    public async Task DispatchAsync(string routingKey, string payload, CancellationToken cancellationToken = default)
    {
        if (!DomainEventTypeMap.TryGetValue(routingKey, out var eventType))
        {
            throw new InvalidOperationException($"Tipo de evento desconocido: {routingKey}");
        }

        var domainEvent = JsonConvert.DeserializeObject(payload, eventType)
            ?? throw new InvalidOperationException("No se pudo deserializar el evento.");

        await mediator.Publish((IDomainEvent)domainEvent, cancellationToken);
    }
}
