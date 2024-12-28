namespace AgendaManager.Application.ResourceTypes.Queries.GetResourceTypesPaginated;

public record GetResourceTypesPaginatedQueryResponse(
    Guid ResponseTypeId,
    Guid? RoleId,
    string Name,
    string Description);
