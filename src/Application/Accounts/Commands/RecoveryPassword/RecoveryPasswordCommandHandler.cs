using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects.EmailAddress;
using AgendaManager.Domain.Users.Enums;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Interfaces;

namespace AgendaManager.Application.Accounts.Commands.RecoveryPassword;

internal class RecoveryPasswordCommandHandler(IUserRepository userRepository)
    : ICommandHandler<RecoveryPasswordCommand>
{
    public async Task<Result> Handle(RecoveryPasswordCommand request, CancellationToken cancellationToken)
    {
        var email = EmailAddress.From(request.Email);
        var user = await userRepository.GetByEmailAsync(email, cancellationToken);

        if (user is null)
        {
            return UserErrors.UserNotFound;
        }

        var userToken = user.CreateUserToken(UserTokenType.PasswordReset);

        return userToken;
    }
}
