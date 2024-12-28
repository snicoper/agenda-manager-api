using AgendaManager.Domain.ResourceManagement.Shared.Enums;

namespace AgendaManager.Application.ResourceManagement.ResourceTypes.Queries.GetResourceTypesPaginated;

public record GetResourceTypesPaginatedQueryResponse(
    Guid ResourceTypeId,
    string Name,
    string Description,
    ResourceCategory Category);
