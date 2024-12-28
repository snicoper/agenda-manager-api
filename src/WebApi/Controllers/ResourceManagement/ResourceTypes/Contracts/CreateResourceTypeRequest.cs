using AgendaManager.Domain.ResourceManagement.Shared.Enums;

namespace AgendaManager.WebApi.Controllers.ResourceManagement.ResourceTypes.Contracts;

public record CreateResourceTypeRequest(string Name, string Description, ResourceCategory Category);
