using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.ResourceManagement.ResourceTypes.Commands.DeleteResourceType;

[Authorize(Permissions = SystemPermissions.ResourceTypes.Delete)]
public record DeleteResourceTypeCommand(Guid ResourceTypeId) : ICommand;
