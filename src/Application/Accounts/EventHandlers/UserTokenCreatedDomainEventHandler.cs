using AgendaManager.Application.Accounts.Interfaces;
using AgendaManager.Application.Common.Abstractions;
using AgendaManager.Application.Common.Exceptions;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Events;
using AgendaManager.Domain.Users.Interfaces;
using Microsoft.Extensions.Logging;

namespace AgendaManager.Application.Accounts.EventHandlers;

public class UserTokenCreatedDomainEventHandler(
    IUserRepository userRepository,
    ISendRecoveryPasswordService emailService,
    ILogger<BaseEventHandler<UserTokenCreatedDomainEvent>> logger)
    : BaseEventHandler<UserTokenCreatedDomainEvent>(logger)
{
    protected override async Task HandleEvent(
        UserTokenCreatedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(notification.UserToken.UserId, cancellationToken);

        if (user is null)
        {
            throw new ApplicationEventHandlerException(UserErrors.UserNotFound.FirstError()?.Description!);
        }

        await emailService.SendAsync(user, notification.UserToken.Token.ToString(), cancellationToken);
    }
}
