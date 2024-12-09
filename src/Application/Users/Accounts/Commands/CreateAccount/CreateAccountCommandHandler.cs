using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Authorization.Services;
using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Responses;
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
    : ICommandHandler<CreateAccountCommand>
{
    public async Task<Result> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        // 1. Password hasher.
        var passwordHash = PasswordHash.FromRaw(request.Password, passwordHasher, passwordPolicy);

        if (passwordHash.IsFailure)
        {
            return passwordHash;
        }

        // 2. Create user.
        var email = EmailAddress.From(request.Email);
        var userId = UserId.Create();

        var resultCreated = await userManager.CreateUserAsync(
            userId: userId,
            email: email,
            passwordHash: passwordHash.Value!,
            firstName: request.FirstName,
            lastName: request.LastName,
            active: request.IsActive,
            isCollaborator: request.IsCollaborator,
            emailConfirmed: request.IsEmailConfirmed,
            cancellationToken: cancellationToken);

        if (resultCreated.IsFailure)
        {
            return resultCreated;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        // 3. Add roles to user.
        foreach (var roleId in request.Roles)
        {
            var addRoleResult = await authorizationService.AddRoleToUserAsync(
                userId: resultCreated.Value?.Id!,
                roleId: RoleId.From(roleId),
                cancellationToken: cancellationToken);

            if (addRoleResult.IsFailure)
            {
                return addRoleResult;
            }
        }

        // 4. Save changes.
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Create();
    }
}
