﻿using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Users.Enums;
using AgendaManager.Domain.Users.Errors;
using AgendaManager.Domain.Users.Interfaces;

namespace AgendaManager.Application.Users.Accounts.Commands.RecoveryPassword;

internal class RecoveryPasswordCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
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

        if (!user.IsActive)
        {
            return UserErrors.UserIsNotActive;
        }

        if (!user.IsEmailConfirmed)
        {
            return UserErrors.EmailIsNotConfirmed;
        }

        var userToken = user.CreateUserToken(UserTokenType.PasswordReset);
        userRepository.Update(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return userToken;
    }
}
