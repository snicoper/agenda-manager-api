using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Infrastructure.Common.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace AgendaManager.Infrastructure.Common.Middlewares;

public class EventualConsistencyMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext httpContext, IPublisher publisher, AppDbContext dbContext)
    {
        var transaction = await dbContext.Database.BeginTransactionAsync();

        httpContext.Response.OnCompleted(
            async () =>
            {
                try
                {
                    if (httpContext.Items["DomainEventsQueue"] is not Queue<IDomainEvent> domainEventsQueue)
                    {
                        return;
                    }

                    while (domainEventsQueue.TryDequeue(out var domainEvent))
                    {
                        await publisher.Publish(domainEvent);
                    }

                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    // Notify the client that even though they got a good response, the changes didn't take place
                    // due to an unexpected error.
                }
                finally
                {
                    await transaction.DisposeAsync();
                }
            });

        await next(httpContext);
    }
}
