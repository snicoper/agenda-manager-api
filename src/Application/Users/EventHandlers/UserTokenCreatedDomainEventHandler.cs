using AgendaManager.Application.Common.Abstractions;
using AgendaManager.Application.Common.Exceptions;
using AgendaManager.Application.Users.Interfaces;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Enums;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Events;
using AgendaManager.Domain.Users.Interfaces;
using Microsoft.Extensions.Logging;

namespace AgendaManager.Application.Users.EventHandlers;

public class UserTokenCreatedDomainEventHandler(
    IUserRepository userRepository,
    ISendRecoveryPasswordService recoveryPasswordService,
    ISendConfirmEmailResentService confirmEmailResentService,
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

        switch (notification.UserToken.Type)
        {
            case UserTokenType.EmailConfirmation:
                await SendEmailConfirmationAsync(user, notification, cancellationToken);
                break;
            case UserTokenType.PasswordReset:
                await SendRecoveryPasswordAsync(user, notification, cancellationToken);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private async Task SendEmailConfirmationAsync(
        User user,
        UserTokenCreatedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        await confirmEmailResentService.SendAsync(user, notification.UserToken.Token.Value, cancellationToken);
    }

    private async Task SendRecoveryPasswordAsync(
        User user,
        UserTokenCreatedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        await recoveryPasswordService.SendAsync(user, notification.UserToken.Token.Value, cancellationToken);
    }
}
