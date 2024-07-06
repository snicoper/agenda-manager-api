using AgendaManager.Application.Common.Abstractions;
using AgendaManager.Domain.Users.Events;
using Microsoft.Extensions.Logging;

namespace AgendaManager.Application.Users.Events;

public class UserCreatedDomainEventHandler(ILogger<BaseEventHandler<UserCreatedDomainEvent>> logger)
    : BaseEventHandler<UserCreatedDomainEvent>(logger)
{
    protected override Task HandleEvent(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
