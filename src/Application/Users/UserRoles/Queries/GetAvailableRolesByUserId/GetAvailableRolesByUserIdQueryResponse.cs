namespace AgendaManager.Application.Users.UserRoles.Queries.GetAvailableRolesByUserId;

public record GetAvailableRolesByUserIdQueryResponse(Guid RoleId, string Name, bool IsAssigned);
