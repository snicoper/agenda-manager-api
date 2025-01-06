using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.ResourceManagement.Shared.Enums;

namespace AgendaManager.Application.ResourceManagement.Resources.Commands.CreateResource;

public record CreateResourceCommand(
    string Name,
    string Description,
    ResourceCategory Category,
    string TextColor,
    string BackgroundColor) : ICommand;
