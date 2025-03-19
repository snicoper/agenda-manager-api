using System.Text.Json;
using AgendaManager.Domain.Users.Events;
using AgendaManager.Infrastructure.Common.Messaging.Interfaces;
using MediatR;
using Newtonsoft.Json;

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

        var domainEvent = JsonConvert.DeserializeObject(payload, eventType)
            ?? throw new InvalidOperationException("No se pudo deserializar el evento.");

        var handlerType = typeof(INotificationHandler<>).MakeGenericType(eventType);

        var handler = serviceProvider.GetService(handlerType)
            ?? throw new InvalidOperationException($"No se encontró handler para el evento: {routingKey}");

        var method = handlerType.GetMethod("Handle")
            ?? throw new InvalidOperationException($"Handler para {routingKey} no implementa Handle");

        await (Task)method.Invoke(handler, [domainEvent, cancellationToken])!;
    }
}
