using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.ResourceManagement.ResourceTypes.Commands.UpdateResourceType;

[Authorize(Permissions = SystemPermissions.ResourceTypes.Update)]
public record UpdateResourceTypeCommand(Guid ResourceTypeId, string Name, string Description) : ICommand;
