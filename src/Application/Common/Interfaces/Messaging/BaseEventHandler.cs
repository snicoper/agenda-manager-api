using MediatR;
using Microsoft.Extensions.Logging;

namespace AgendaManager.Application.Common.Interfaces.Messaging;

public abstract class BaseEventHandler<TEvent>(ILogger<BaseEventHandler<TEvent>> logger)
    : INotificationHandler<TEvent>
    where TEvent : INotification
{
    public async Task Handle(TEvent notification, CancellationToken cancellationToken)
    {
        BeforeHandle(notification);

        await HandleEvent(notification, cancellationToken);

        AfterHandle(notification);
    }

    protected abstract Task HandleEvent(TEvent notification, CancellationToken cancellationToken);

    private void AfterHandle(TEvent notification)
    {
        logger.LogInformation("Event handled: {EventName}", typeof(TEvent).Name);
    }

    private void BeforeHandle(TEvent notification)
    {
        logger.LogInformation("Handling event: {EventName}", typeof(TEvent).Name);
    }
}
