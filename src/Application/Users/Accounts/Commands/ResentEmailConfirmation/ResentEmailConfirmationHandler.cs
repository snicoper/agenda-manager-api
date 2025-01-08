using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.Enums;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Interfaces;

namespace AgendaManager.Application.Users.Accounts.Commands.ResentEmailConfirmation;

internal class ResentEmailConfirmationHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<ResentEmailConfirmationCommand>
{
    public async Task<Result> Handle(ResentEmailConfirmationCommand request, CancellationToken cancellationToken)
    {
        // Get user by email and check if it exists.
        var user = await userRepository.GetByEmailWithTokensAsync(EmailAddress.From(request.Email), cancellationToken);
        if (user is null)
        {
            return UserErrors.UserNotFound;
        }

        // Check if user is active.
        if (!user.IsActive)
        {
            return UserErrors.UserIsNotActive;
        }

        // Check if user have email confirmed.
        if (user.IsEmailConfirmed)
        {
            return UserErrors.UserAlreadyConfirmedEmail;
        }

        // Remove all tokens of type EmailConfirmation.
        List<UserToken> userTokens = [];
        userTokens.AddRange(user.Tokens.Where(userToken => userToken.Type == UserTokenType.EmailConfirmation));
        userTokens.ForEach(ut => user.RemoveUserToken(ut));

        // Create new token and save it.
        user.CreateUserToken(UserTokenType.EmailConfirmation);
        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
