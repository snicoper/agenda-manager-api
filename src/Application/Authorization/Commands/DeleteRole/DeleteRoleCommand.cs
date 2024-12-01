using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.Authorization.Commands.DeleteRole;

[Authorize(Permissions = SystemPermissions.Roles.Delete)]
public record DeleteRoleCommand(Guid RoleId) : ICommand;
