using AgendaManager.Application.Common.Abstractions;
using AgendaManager.Application.Common.Exceptions;
using AgendaManager.Application.Users.Interfaces;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.Enums;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Events;
using AgendaManager.Domain.Users.Interfaces;
using Microsoft.Extensions.Logging;

namespace AgendaManager.Application.Users.EventHandlers;

public class UserTokenCreatedDomainEventHandler(
    IUserRepository userRepository,
    ISendRequestPasswordResetService requestPasswordResetService,
    ISendResentEmailConfirmationService resentEmailConfirmationService,
    ISendConfirmAccountService confirmAccountService,
    ILogger<BaseEventHandler<UserTokenCreatedDomainEvent>> logger)
    : BaseEventHandler<UserTokenCreatedDomainEvent>(logger)
{
    protected override async Task HandleEvent(
        UserTokenCreatedDomainEvent notification,
        CancellationToken cancellationToken)
    {
        // Get the user with token and check if it exists.
        var result = await userRepository.GetUserWithSpecificTokenAsync(notification.UserTokenId, cancellationToken);

        if (result is null)
        {
            throw new ApplicationEventHandlerException(UserErrors.UserNotFound.FirstError()?.Description!);
        }

        var (user, token) = result.Value;

        // Send the email based on the token type.
        switch (token?.Type)
        {
            case UserTokenType.EmailConfirmation:
                await SendResentEmailConfirmationAsync(user, token, cancellationToken);
                break;
            case UserTokenType.PasswordReset:
                await SendRequestPasswordResetAsync(user, token, cancellationToken);
                break;
            case UserTokenType.AdminCreatedAccount:
                await SendConfirmAccountEmailAsync(user, token, cancellationToken);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private async Task SendResentEmailConfirmationAsync(
        User user,
        UserToken userToken,
        CancellationToken cancellationToken)
    {
        await resentEmailConfirmationService.SendAsync(user, userToken.Token.Value, cancellationToken);
    }

    private async Task SendRequestPasswordResetAsync(
        User user,
        UserToken userToken,
        CancellationToken cancellationToken)
    {
        await requestPasswordResetService.SendAsync(user, userToken.Token.Value, cancellationToken);
    }

    private async Task SendConfirmAccountEmailAsync(
        User user,
        UserToken userToken,
        CancellationToken cancellationToken)
    {
        await confirmAccountService.SendAsync(user, userToken.Token.Value, cancellationToken);
    }
}
