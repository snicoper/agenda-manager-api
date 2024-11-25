using MediatR;
using Microsoft.Extensions.Logging;

namespace AgendaManager.Application.Common.Abstractions;

public abstract class BaseEventHandler<TEvent>() : INotificationHandler<TEvent>
    where TEvent : INotification
{
    protected BaseEventHandler(ILogger<BaseEventHandler<TEvent>> logger)
        : this()
    {
        Logger = logger;
    }

    protected ILogger<BaseEventHandler<TEvent>> Logger { get; } = null!;

    public async Task Handle(TEvent notification, CancellationToken cancellationToken)
    {
        BeforeHandle(notification);

        await HandleEvent(notification, cancellationToken);

        AfterHandle(notification);
    }

    protected abstract Task HandleEvent(TEvent notification, CancellationToken cancellationToken);

    private void AfterHandle(TEvent notification)
    {
        Logger.LogInformation("Event handled: {EventName} in {EventHandleName}", typeof(TEvent).Name, GetType().Name);
    }

    private void BeforeHandle(TEvent notification)
    {
        Logger.LogInformation("Handling event: {EventName} in {EventHandleName}", typeof(TEvent).Name, GetType().Name);
    }
}
