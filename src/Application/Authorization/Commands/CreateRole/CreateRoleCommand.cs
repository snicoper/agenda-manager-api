using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.Authorization.Commands.CreateRole;

[Authorize(Permissions = SystemPermissions.Roles.Create)]
public record CreateRoleCommand(string Name, string Description) : ICommand<CreateRoleCommandResponse>;
