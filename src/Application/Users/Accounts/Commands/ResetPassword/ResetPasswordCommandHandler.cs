using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Application.Users.Accounts.Commands.ResetPassword;

internal class ResetPasswordCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IPasswordPolicy passwordPolicy,
    IUnitOfWork unitOfWork)
    : ICommandHandler<ResetPasswordCommand>
{
    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByTokenValueWithTokensAsync(request.Token, cancellationToken);

        if (user is null)
        {
            return UserTokenErrors.UserTokenNotFoundOrExpired;
        }

        var userToken = user.Tokens.First(x => x.Token.Value == request.Token);

        if (userToken.IsExpired)
        {
            await RemoveTokenFromUserAsync(user, userToken, cancellationToken);

            return UserTokenErrors.UserTokenNotFoundOrExpired;
        }

        var newPassword = PasswordHash.FromRaw(request.NewPassword, passwordHasher, passwordPolicy);

        if (newPassword.IsFailure)
        {
            await RemoveTokenFromUserAsync(user, userToken, cancellationToken);

            return newPassword;
        }

        user.UpdatePassword(newPassword.Value!);
        await RemoveTokenFromUserAsync(user, userToken, cancellationToken);

        return Result.Success();
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
