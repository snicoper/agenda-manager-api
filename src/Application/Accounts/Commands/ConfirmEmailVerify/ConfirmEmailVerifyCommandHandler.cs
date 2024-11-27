using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Interfaces;

namespace AgendaManager.Application.Accounts.Commands.ConfirmEmailVerify;

internal class ConfirmEmailVerifyCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<ConfirmEmailVerifyCommand>
{
    public async Task<Result> Handle(ConfirmEmailVerifyCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByTokenValueWithTokensAsync(request.Token, cancellationToken);

        if (user is null)
        {
            return UserErrors.UserNotFound;
        }

        if (user.IsEmailConfirmed)
        {
            return UserErrors.UserAlreadyConfirmedEmail;
        }

        var userToken = user.Tokens.First(x => x.Token.Value == request.Token);
        user.ConfirmEmail();
        user.RemoveUserToken(userToken);

        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Create();
    }
}
