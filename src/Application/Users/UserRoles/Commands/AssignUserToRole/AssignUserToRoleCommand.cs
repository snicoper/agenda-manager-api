using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.Users.UserRoles.Commands.AssignUserToRole;

[Authorize(Permissions = SystemPermissions.Roles.Update)]
public record AssignUserToRoleCommand(Guid RoleId, Guid UserId) : ICommand;
