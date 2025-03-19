using AgendaManager.Domain.Users.Events;
using AgendaManager.Infrastructure.Common.Messaging.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
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

        var mediator = serviceProvider.GetRequiredService<IMediator>();

        await mediator.Publish((INotification)domainEvent, cancellationToken);
    }
}
