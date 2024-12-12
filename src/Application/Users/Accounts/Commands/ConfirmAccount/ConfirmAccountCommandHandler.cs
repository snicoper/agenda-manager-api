using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Enums;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.Services;

namespace AgendaManager.Application.Users.Accounts.Commands.ConfirmAccount;

internal class ConfirmAccountCommandHandler(
    IUserRepository userRepository,
    UserManager userManager,
    IUnitOfWork unitOfWork)
    : ICommandHandler<ConfirmAccountCommand>
{
    public async Task<Result> Handle(ConfirmAccountCommand request, CancellationToken cancellationToken)
    {
        // 1. Check if user exists.
        var user = await userRepository.GetByTokenValueWithTokensAsync(request.Token, cancellationToken);

        if (user == null)
        {
            return UserTokenErrors.UserTokenNotFoundOrExpired;
        }

        // 2. Hash and update the password.
        userManager.UpdatePassword(user, request.NewPassword);

        // 3. Remove the token.
        user.ConsumeUserToken(user.Tokens.First(t => t.Type == UserTokenType.AdminCreatedAccount).Id, request.Token);

        // 4. Save changes.
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
