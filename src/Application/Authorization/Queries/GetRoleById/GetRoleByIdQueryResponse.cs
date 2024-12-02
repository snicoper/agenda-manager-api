namespace AgendaManager.Application.Authorization.Queries.GetRoleById;

public record GetRoleByIdQueryResponse(Guid Id, string Name, string Description, bool IsEditable);
