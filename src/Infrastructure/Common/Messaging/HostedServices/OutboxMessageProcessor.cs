using AgendaManager.Domain.Common.Messaging.Interfaces;
using AgendaManager.Infrastructure.Common.Messaging.Interfaces;
using AgendaManager.Infrastructure.Common.Persistence;
using Microsoft.Extensions.Logging;

namespace AgendaManager.Infrastructure.Common.Messaging.HostedServices;

public class OutboxMessageProcessor(
    IRabbitMqClient rabbitMqClient,
    ILogger<OutboxMessageProcessor> logger)
{
    public async Task ProcessMessagesAsync(
        IAppDbContext dbContext,
        IOutboxMessageRepository outboxRepository,
        CancellationToken cancellationToken = default)
    {
        var messages = await outboxRepository.GetMessagesForPublishAsync(cancellationToken);

        foreach (var message in messages.Where(message => message.ShouldRetry()))
        {
            try
            {
                var routingKey = message.Type;
                var payload = message.Payload;

                await rabbitMqClient.PublishAsync(routingKey, payload, cancellationToken);

                message.MarkAsPublished();
                logger.LogInformation("Message published: {RoutingKey} - {Message}", routingKey, payload);
            }
            catch (Exception ex)
            {
                message.MarkAsFailed(ex.Message);
                logger.LogError(ex, "Failed to dispatch event {RoutingKey}", message.Id);
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
