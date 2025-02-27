﻿using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.Services;

namespace AgendaManager.Application.Users.Accounts.Commands.ResetPassword;

internal class ResetPasswordCommandHandler(
    IUserRepository userRepository,
    UserManager userManager,
    IUnitOfWork unitOfWork)
    : ICommandHandler<ResetPasswordCommand>
{
    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        // Check if user with token exists.
        var user = await userRepository.GetByTokenValueWithTokensAsync(request.Token, cancellationToken);
        if (user is null)
        {
            return UserTokenErrors.UserTokenNotFoundOrExpired;
        }

        // Check if token is expired and remove it.
        var userToken = user.Tokens.First(x => x.Token.Value == request.Token);
        if (userToken.IsExpired)
        {
            await RemoveTokenFromUserAsync(user, userToken, cancellationToken);

            return UserTokenErrors.UserTokenNotFoundOrExpired;
        }

        // Update the password and remove the token.
        var updatePasswordResult = userManager.UpdatePassword(user, request.NewPassword);
        if (updatePasswordResult.IsFailure)
        {
            await RemoveTokenFromUserAsync(user, userToken, cancellationToken);

            return updatePasswordResult;
        }

        // Remove the token.
        await RemoveTokenFromUserAsync(user, userToken, cancellationToken);

        return Result.NoContent();
    }

    private async Task RemoveTokenFromUserAsync(User user, UserToken? userToken, CancellationToken cancellationToken)
    {
        if (userToken is null)
        {
            return;
        }

        user.RemoveUserToken(userToken);
        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
