using AgendaManager.Domain.Users.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AgendaManager.Application.Users.Events;

public class UserCreatedDomainEventHandler(ILogger<UserCreatedDomainEventHandler> logger)
    : INotificationHandler<UserCreatedDomainEvent>
{
    public Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("User created: {UserId}", notification.UserId);

        return Task.CompletedTask;
    }
}
