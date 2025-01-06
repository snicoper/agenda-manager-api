using AgendaManager.Domain.ResourceManagement.Shared.Enums;

namespace AgendaManager.WebApi.Controllers.ResourceManagement.Resources.Contracts;

public record CreateResourceRequest(
    string Name,
    string Description,
    ResourceCategory Category,
    string TextColor,
    string BackgroundColor);
