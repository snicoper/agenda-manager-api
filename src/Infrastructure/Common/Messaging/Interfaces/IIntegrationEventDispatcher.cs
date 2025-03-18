namespace AgendaManager.Infrastructure.Common.Messaging.Interfaces;

public interface IIntegrationEventDispatcher
{
    Task DispatchAsync(string routingKey, string payload, CancellationToken cancellationToken = default);
}
