using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Application.Users.Accounts.Commands.ConfirmRecoveryPassword;

internal class ConfirmRecoveryPasswordCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IPasswordPolicy passwordPolicy,
    IUnitOfWork unitOfWork)
    : ICommandHandler<ConfirmRecoveryPasswordCommand>
{
    public async Task<Result> Handle(ConfirmRecoveryPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByTokenValueWithTokensAsync(request.Token, cancellationToken);

        if (user is null)
        {
            return UserTokenErrors.UserTokenNotFoundOrExpired;
        }

        var userToken = user.Tokens.First(x => x.Token.Value == request.Token);

        if (userToken.IsExpired)
        {
            return UserTokenErrors.UserTokenNotFoundOrExpired;
        }

        var newPassword = PasswordHash.FromRaw(request.NewPassword, passwordHasher, passwordPolicy);

        if (newPassword.IsFailure)
        {
            return newPassword;
        }

        user.UpdatePassword(newPassword.Value!);
        user.RemoveUserToken(userToken);
        userRepository.Update(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
