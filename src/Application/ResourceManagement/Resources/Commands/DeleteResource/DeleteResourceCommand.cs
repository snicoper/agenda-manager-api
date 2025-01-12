using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.ResourceManagement.Resources.Commands.DeleteResource;

[Authorize(Permissions = SystemPermissions.Resources.Delete)]
public record DeleteResourceCommand(Guid ResourceId) : ICommand;
