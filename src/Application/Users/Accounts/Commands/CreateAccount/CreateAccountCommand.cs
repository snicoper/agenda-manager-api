using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.Users.Accounts.Commands.CreateAccount;

[Authorize(Permissions = SystemPermissions.Users.Create)]
public record CreateAccountCommand(
    string Email,
    string FirstName,
    string LastName,
    string Password,
    string PasswordConfirmation,
    List<Guid> Roles,
    bool IsActive,
    bool IsEmailConfirmed,
    bool IsCollaborator) : ICommand;
