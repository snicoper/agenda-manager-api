using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.Authorization.Commands.UpdatePermissionForRole;

[Authorize(Permissions = SystemPermissions.Roles.Update)]
public record UpdatePermissionForRoleCommand(Guid RoleId, Guid PermissionId, bool IsAssigned) : ICommand;
