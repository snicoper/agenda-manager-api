﻿using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Authorization.Services;
using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.Utils;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Users.Enums;
using AgendaManager.Domain.Users.Services;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Application.Users.Accounts.Commands.CreateAccount;

internal class CreateAccountCommandHandler(
    UserManager userManager,
    AuthorizationService authorizationService,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateAccountCommand, CreateAccountCommandResponse>
{
    public async Task<Result<CreateAccountCommandResponse>> Handle(
        CreateAccountCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Generate the raw password.
        var passwordRaw = PasswordGenerator.GeneratePassword();

        // 2. Create user.
        var email = EmailAddress.From(request.Email);
        var userId = UserId.Create();

        var userResultCreated = await userManager.CreateUserAsync(
            userId: userId,
            email: email,
            passwordRaw: passwordRaw,
            firstName: request.FirstName,
            lastName: request.LastName,
            isCollaborator: request.IsCollaborator,
            cancellationToken: cancellationToken);

        if (userResultCreated.IsFailure)
        {
            return userResultCreated.MapTo<CreateAccountCommandResponse>();
        }

        // 3. Create user token.
        var tokenResult = userResultCreated.Value?.CreateUserToken(UserTokenType.AdminCreatedAccount);

        if (tokenResult!.IsFailure)
        {
            return tokenResult.MapTo<CreateAccountCommandResponse>();
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        // 4. Add roles to user.
        foreach (var roleId in request.Roles)
        {
            var addRoleResult = await authorizationService.AddRoleToUserAsync(
                userId: userId,
                roleId: RoleId.From(roleId),
                cancellationToken: cancellationToken);

            if (addRoleResult.IsFailure)
            {
                return addRoleResult.MapToValue<CreateAccountCommandResponse>();
            }
        }

        // 5. Save changes.
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var resultResponse = new CreateAccountCommandResponse(userId.Value);

        return Result.Create(resultResponse);
    }
}
