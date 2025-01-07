using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.ResourceManagement.Resources.Commands.CreateResource;

[Authorize(Permissions = SystemPermissions.Resources.Create)]
public record CreateResourceCommand(
    Guid? UserId,
    Guid ResourceTypeId,
    string Name,
    string Description,
    string TextColor,
    string BackgroundColor) : ICommand;
