namespace AgendaManager.WebApi.Controllers.ResourceManagement.Resources.Contracts;

public record UpdateResourceRequest(string Name, string Description, string TextColor, string BackgroundColor);
