using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Infrastructure.Common.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AgendaManager.Infrastructure.Common.Middlewares;

public class EventualConsistencyMiddleware(RequestDelegate next)
{
    public const string DomainEventsQueueKey = "DomainEventsQueue";

    public async Task InvokeAsync(
        HttpContext httpContext,
        IPublisher publisher,
        AppDbContext dbContext,
        ILogger<EventualConsistencyMiddleware> logger)
    {
        var transaction = await dbContext.Database.BeginTransactionAsync();

        httpContext.Response.OnCompleted(
            async () =>
            {
                try
                {
                    if (httpContext.Items[DomainEventsQueueKey] is not Queue<IDomainEvent> domainEventsQueue)
                    {
                        return;
                    }

                    while (domainEventsQueue.TryDequeue(out var nextEvent))
                    {
                        await publisher.Publish(nextEvent);
                    }

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    // Notify the client that even though they got a good response, the changes didn't take place
                    // due to an unexpected error.
                    logger.LogError(ex, "Error while publishing domain events");
                }
                finally
                {
                    await transaction.DisposeAsync();
                }
            });

        await next(httpContext);
    }
}
