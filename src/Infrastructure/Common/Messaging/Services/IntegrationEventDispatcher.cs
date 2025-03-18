using System.Text.Json;
using AgendaManager.Domain.Users.Events;
using AgendaManager.Infrastructure.Common.Messaging.Interfaces;
using MediatR;

namespace AgendaManager.Infrastructure.Common.Messaging.Services;

public class IntegrationEventDispatcher(IServiceProvider serviceProvider) : IIntegrationEventDispatcher
{
    private readonly Dictionary<string, Type> _eventTypeMap = new()
    {
        { nameof(UserDeactivatedDomainEvent), typeof(UserDeactivatedDomainEvent) }
    };

    public async Task DispatchAsync(string routingKey, string payload, CancellationToken cancellationToken = default)
    {
        if (!_eventTypeMap.TryGetValue(routingKey, out var eventType))
        {
            throw new InvalidOperationException($"Tipo de evento desconocido: {routingKey}");
        }

        var domainEvent = JsonSerializer.Deserialize(payload, eventType);
        if (domainEvent is null)
        {
            throw new InvalidOperationException("No se pudo deserializar el evento.");
        }

        var handlerType = typeof(INotificationHandler<>).MakeGenericType(eventType);

        var handler = serviceProvider.GetService(handlerType);
        if (handler is null)
        {
            throw new InvalidOperationException($"No se encontró handler para el evento: {routingKey}");
        }

        var method = handlerType.GetMethod("Handle");
        if (method is null)
        {
            throw new InvalidOperationException($"Handler para {routingKey} no implementa HandleAsync");
        }

        await (Task)method.Invoke(handler, [domainEvent, cancellationToken])!;
    }
}
