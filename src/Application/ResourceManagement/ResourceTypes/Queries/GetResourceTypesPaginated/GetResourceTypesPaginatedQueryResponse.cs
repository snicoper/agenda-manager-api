namespace AgendaManager.Application.ResourceManagement.ResourceTypes.Queries.GetResourceTypesPaginated;

public record GetResourceTypesPaginatedQueryResponse(
    Guid ResponseTypeId,
    Guid? RoleId,
    string Name,
    string Description);
