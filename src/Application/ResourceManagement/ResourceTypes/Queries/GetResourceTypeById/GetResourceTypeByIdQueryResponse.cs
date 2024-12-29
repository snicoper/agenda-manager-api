using AgendaManager.Domain.ResourceManagement.Shared.Enums;

namespace AgendaManager.Application.ResourceManagement.ResourceTypes.Queries.GetResourceTypeById;

public record GetResourceTypeByIdQueryResponse(
    Guid ResourceTypeId,
    string Name,
    string Description,
    ResourceCategory Category);
