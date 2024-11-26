using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Application.Accounts.Commands.ConfirmRecoveryPassword;

internal class ConfirmRecoveryPasswordCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IPasswordPolicy passwordPolicy)
    : ICommandHandler<ConfirmRecoveryPasswordCommand>
{
    public async Task<Result> Handle(ConfirmRecoveryPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByTokenValueWithTokensAsync(request.TokenValue, cancellationToken);

        if (user is null)
        {
            return UserTokenErrors.UserTokenNotFound;
        }

        var newPassword = PasswordHash.FromRaw(request.NewPassword, passwordHasher, passwordPolicy);

        if (newPassword.IsFailure)
        {
            return newPassword;
        }

        user.UpdatePassword(newPassword.Value!);
        user.RemoveUserToken(user.Tokens.First(x => x.Token.Value == request.TokenValue));
        userRepository.Update(user);

        // await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
