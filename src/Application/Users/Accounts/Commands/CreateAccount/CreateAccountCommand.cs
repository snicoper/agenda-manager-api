using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.Users.Accounts.Commands.CreateAccount;

[Authorize(Permissions = SystemPermissions.Users.Create)]
public record CreateAccountCommand(
    string Email,
    string FirstName,
    string LastName,
    List<Guid> Roles,
    bool IsCollaborator) : ICommand<CreateAccountCommandResponse>;
