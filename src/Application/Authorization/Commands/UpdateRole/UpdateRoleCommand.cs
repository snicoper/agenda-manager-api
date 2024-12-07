using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.Authorization.Commands.UpdateRole;

[Authorize(Permissions = SystemPermissions.Roles.Update)]
public record UpdateRoleCommand(Guid RoleId, string Name, string Description) : ICommand;
