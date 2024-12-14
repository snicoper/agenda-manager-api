using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.Users.Accounts.Commands.ToggleIsCollaborator;

[Authorize(Permissions = SystemPermissions.Users.Update)]
public record ToggleIsCollaboratorCommand(Guid UserId) : ICommand;
