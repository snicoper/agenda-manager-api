namespace AgendaManager.Infrastructure.Common.Messaging.Interfaces;

public interface IRabbitMqClient
{
    Task PublishAsync(
        string routingKey,
        string message,
        CancellationToken cancellationToken = default);
}
