using AgendaManager.Application.Common.Interfaces.Messaging;
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
        // Generate the raw password.
        var passwordRaw = PasswordGenerator.GeneratePassword();

        // Create user.
        var email = EmailAddress.From(request.Email);
        var userId = UserId.Create();

        var userResultCreated = await userManager.CreateUserAsync(
            userId: userId,
            email: email,
            passwordRaw: passwordRaw,
            firstName: request.FirstName,
            lastName: request.LastName,
            cancellationToken: cancellationToken);

        if (userResultCreated.IsFailure)
        {
            return userResultCreated.MapTo<CreateAccountCommandResponse>();
        }

        // Create user token.
        var tokenResult = userResultCreated.Value?.CreateUserToken(UserTokenType.AdminCreatedAccount);
        if (tokenResult!.IsFailure)
        {
            return tokenResult.MapTo<CreateAccountCommandResponse>();
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        // Add roles to user.
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

        // Save changes.
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // Return the response.
        var resultResponse = new CreateAccountCommandResponse(userId.Value);

        return Result.Create(resultResponse);
    }
}
