using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.Users.UserRoles.Commands.UnAssignedUserFromRole;

[Authorize(Permissions = SystemPermissions.Roles.Update)]
public record UnAssignedUserFromRoleCommand(Guid RoleId, Guid UserId) : ICommand;
