namespace AgendaManager.Application.Users.UserRoles.Queries.GetUsersNotInRoleIdPaginated;

public record GetUsersNotInRoleIdPaginatedCommandResponse(Guid UserId, string Email);
