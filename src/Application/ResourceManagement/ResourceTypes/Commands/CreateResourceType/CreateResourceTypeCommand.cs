using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;
using AgendaManager.Domain.ResourceManagement.Shared.Enums;

namespace AgendaManager.Application.ResourceManagement.ResourceTypes.Commands.CreateResourceType;

[Authorize(Permissions = SystemPermissions.ResourceTypes.Create)]
public record CreateResourceTypeCommand(string Name, string Description, ResourceCategory Category) : ICommand;
