using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Interfaces;

namespace AgendaManager.Application.Users.Accounts.Commands.VerifyEmail;

internal class VerifyEmailCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<VerifyEmailCommand>
{
    public async Task<Result> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByTokenValueWithTokensAsync(request.Token, cancellationToken);

        if (user is null)
        {
            return UserErrors.UserNotFound;
        }

        var userToken = user.Tokens.First(x => x.Token.Value == request.Token);

        if (!user.IsActive)
        {
            await RemoveTokenFromUserAsync(user, userToken, cancellationToken);

            return UserErrors.UserIsNotActive;
        }

        if (user.IsEmailConfirmed)
        {
            await RemoveTokenFromUserAsync(user, userToken, cancellationToken);

            return UserErrors.UserAlreadyConfirmedEmail;
        }

        if (userToken.Token.IsExpired)
        {
            await RemoveTokenFromUserAsync(user, userToken, cancellationToken);

            return UserTokenErrors.TokenHasExpired;
        }

        user.ConfirmEmail();
        await RemoveTokenFromUserAsync(user, userToken, cancellationToken);

        return Result.Create();
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
