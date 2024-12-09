using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Authorization.Services;
using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.Utils;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Users.Interfaces;
using AgendaManager.Domain.Users.Services;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Application.Users.Accounts.Commands.CreateAccount;

internal class CreateAccountCommandHandler(
    IPasswordHasher passwordHasher,
    IPasswordPolicy passwordPolicy,
    UserManager userManager,
    AuthorizationService authorizationService,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateAccountCommand, CreateAccountCommandResponse>
{
    public async Task<Result<CreateAccountCommandResponse>> Handle(
        CreateAccountCommand request,
        CancellationToken cancellationToken)
    {
        // 1. Password hasher.
        var passwordRaw = PasswordGenerator.GeneratePassword();
        var passwordHash = PasswordHash.FromRaw(passwordRaw, passwordHasher, passwordPolicy);

        if (passwordHash.IsFailure)
        {
            return passwordHash.MapTo<CreateAccountCommandResponse>();
        }

        // 2. Create user.
        var email = EmailAddress.From(request.Email);
        var userId = UserId.Create();

        var userResultCreated = await userManager.CreateUserAsync(
            userId: userId,
            email: email,
            passwordHash: passwordHash.Value!,
            firstName: request.FirstName,
            lastName: request.LastName,
            active: request.IsActive,
            isCollaborator: request.IsCollaborator,
            emailConfirmed: request.IsEmailConfirmed,
            cancellationToken: cancellationToken);

        if (userResultCreated.IsFailure)
        {
            return userResultCreated.MapTo<CreateAccountCommandResponse>();
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        // 3. Add roles to user.
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

        // 4. Save changes.
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var resultResponse = new CreateAccountCommandResponse(userId.Value);

        return Result.Create(resultResponse);
    }
}
