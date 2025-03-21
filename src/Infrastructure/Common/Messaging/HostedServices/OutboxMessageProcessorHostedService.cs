using AgendaManager.Domain.Common.Messaging;
using AgendaManager.Domain.Common.Messaging.Interfaces;
using AgendaManager.Infrastructure.Common.Messaging.Interfaces;
using AgendaManager.Infrastructure.Common.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AgendaManager.Infrastructure.Common.Messaging.HostedServices;

public class OutboxMessageProcessorHostedService(
    IServiceScopeFactory scopeFactory,
    IRabbitMqClient rabbitMqClient,
    ILogger<OutboxMessageProcessorHostedService> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Outbox Processor started");

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var outboxMessageRepository = scope.ServiceProvider.GetRequiredService<IOutboxMessageRepository>();
                var messages = await outboxMessageRepository.GetMessagesForPublishAsync(cancellationToken);

                await ProcessMessagesAsync(messages, cancellationToken);

                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Critical error in OutboxMessageProcessorHostedService");
            }

            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
        }

        logger.LogInformation("Outbox Processor stopped");
    }

    private async Task ProcessMessagesAsync(List<OutboxMessage> messages, CancellationToken cancellationToken)
    {
        foreach (var message in messages.Where(message => message.ShouldRetry()))
        {
            var routingKey = message.Type;
            var payload = message.Payload;

            try
            {
                await rabbitMqClient.PublishAsync(routingKey, payload, cancellationToken);
                message.MarkAsPublished();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error publishing OutboxMessage Id: {Id}", message.Id);
                message.MarkAsFailed(ex.Message);
            }
        }
    }
}
