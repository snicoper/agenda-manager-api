using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects.EmailAddress;
using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.Enums;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Interfaces;

namespace AgendaManager.Application.Accounts.Commands.ResendEmailCode;

internal class ResendEmailCodeHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<ResendEmailCodeCommand>
{
    public async Task<Result> Handle(ResendEmailCodeCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailWithTokensAsync(EmailAddress.From(request.Email), cancellationToken);

        if (user is null)
        {
            return UserErrors.UserNotFound;
        }

        if (user.IsEmailConfirmed)
        {
            return UserErrors.UserAlreadyConfirmedEmail;
        }

        List<UserToken> userTokens = [];
        userTokens.AddRange(user.Tokens.Where(userToken => userToken.Type == UserTokenType.EmailConfirmation));
        userTokens.ForEach(ut => user.RemoveUserToken(ut));

        user.CreateUserToken(UserTokenType.EmailConfirmation);
        userRepository.Update(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
