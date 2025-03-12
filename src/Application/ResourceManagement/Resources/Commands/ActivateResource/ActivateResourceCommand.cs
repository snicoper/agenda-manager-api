using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.ResourceManagement.Resources.Commands.ActivateResource;

[Authorize(Permissions = SystemPermissions.Resources.Update)]
public record ActivateResourceCommand(Guid ResourceId) : ICommand;
