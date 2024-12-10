using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Application.Users.Accounts.Commands.AccountConfirmation;

internal class AccountConfirmationCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IPasswordPolicy passwordPolicy,
    IUnitOfWork unitOfWork)
    : ICommandHandler<AccountConfirmationCommand>
{
    public async Task<Result> Handle(AccountConfirmationCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByTokenValueWithTokensAsync(request.Token, cancellationToken);

        if (user == null)
        {
            return UserTokenErrors.UserTokenNotFoundOrExpired;
        }

        var passwordHashResult = PasswordHash.FromRaw(request.NewPassword, passwordHasher, passwordPolicy);

        if (passwordHashResult.IsFailure)
        {
            return passwordHashResult;
        }

        user.UpdatePassword(passwordHashResult.Value!);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
