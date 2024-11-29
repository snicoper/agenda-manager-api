namespace AgendaManager.Application.Authorization.Queries.GetRolesPaginated;

public record GetRolesPaginatedQueryResponse(Guid Id, string Name, string Description, bool IsEditable);
