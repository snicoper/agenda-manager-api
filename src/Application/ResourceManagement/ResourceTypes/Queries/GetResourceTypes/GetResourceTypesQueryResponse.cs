using AgendaManager.Domain.ResourceManagement.Shared.Enums;

namespace AgendaManager.Application.ResourceManagement.ResourceTypes.Queries.GetResourceTypes;

public record GetResourceTypesQueryResponse(Guid ResourceTypeId, string Name, ResourceCategory Category);
