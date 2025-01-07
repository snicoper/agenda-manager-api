namespace AgendaManager.WebApi.Controllers.ResourceManagement.Resources.Contracts;

public record CreateResourceRequest(
    Guid? UserId,
    Guid ResourceTypeId,
    string Name,
    string Description,
    string TextColor,
    string BackgroundColor);
