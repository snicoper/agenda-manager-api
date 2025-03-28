﻿using AgendaManager.Domain.Common.Messaging.Interfaces;
using AgendaManager.Infrastructure.Common.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AgendaManager.Infrastructure.Common.Messaging.HostedServices;

internal sealed class OutboxMessageProcessorHostedService(
    IServiceScopeFactory scopeFactory,
    ILogger<OutboxMessageProcessorHostedService> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancelationToken)
    {
        logger.LogInformation("OutboxMessageProcessorHostedService started.");

        using var scope = scopeFactory.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
        var outboxRepository = scope.ServiceProvider.GetRequiredService<IOutboxMessageRepository>();
        var processor = scope.ServiceProvider.GetRequiredService<OutboxMessageProcessor>();

        while (!cancelationToken.IsCancellationRequested)
        {
            await processor.ProcessMessagesAsync(dbContext, outboxRepository, cancelationToken);

            await Task.Delay(TimeSpan.FromSeconds(10), cancelationToken);
        }

        logger.LogInformation("OutboxMessageProcessorHostedService stopped.");
    }
}
