using AgendaManager.Application.Common.Interfaces.Messaging;

namespace AgendaManager.Application.Authorization.Commands.UpdatePermissionForRole;

public record UpdatePermissionForRoleCommand(Guid RoleId, Guid PermissionId, bool IsAssigned) : ICommand;
