using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Users.Enums;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Interfaces;

namespace AgendaManager.Application.Users.Accounts.Commands.RequestPasswordReset;

internal class RequestPasswordResetCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<RequestPasswordResetCommand>
{
    public async Task<Result> Handle(RequestPasswordResetCommand request, CancellationToken cancellationToken)
    {
        // Get user by email and check if it exists.
        var email = EmailAddress.From(request.Email);
        var user = await userRepository.GetByEmailAsync(email, cancellationToken);

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
        if (!user.IsEmailConfirmed)
        {
            return UserErrors.EmailIsNotConfirmed;
        }

        // Create the token and save it.
        var userToken = user.CreateUserToken(UserTokenType.PasswordReset);
        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return userToken;
    }
}
