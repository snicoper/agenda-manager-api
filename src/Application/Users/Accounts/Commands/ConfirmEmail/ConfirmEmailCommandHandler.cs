using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Users.Enums;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Application.Users.Accounts.Commands.ConfirmEmail;

internal class ConfirmEmailCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<ConfirmEmailCommand>
{
    public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        // Get user by id and check if it exists.
        var user = await userRepository.GetByIdWithTokensAsync(UserId.From(request.UserId), cancellationToken);

        if (user is null)
        {
            return UserErrors.UserNotFound;
        }

        // Check if user have token to confirm email and delete.
        var userToken = user.Tokens.FirstOrDefault(x => x.Type == UserTokenType.EmailConfirmation);

        if (userToken is not null)
        {
            user.ConsumeUserToken(userToken.Id, userToken.Token.Value);
        }

        // Check if user is active and confirm email.
        if (user.IsEmailConfirmed)
        {
            return Result.Success();
        }

        // Confirm the email and save changes.
        user.ConfirmEmail();
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.NoContent();
    }
}
