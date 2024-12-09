using AgendaManager.Application.Common.Abstractions;
using AgendaManager.Domain.Users.Events;
using Microsoft.Extensions.Logging;

namespace AgendaManager.Application.Users.EventHandlers;

public class UserCreatedDomainEventHandler(ILogger<UserCreatedDomainEventHandler> logger)
    : BaseEventHandler<UserCreatedDomainEvent>(logger)
{
    protected override Task HandleEvent(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // TODO: Send email to user
        return Task.CompletedTask;
    }
}
