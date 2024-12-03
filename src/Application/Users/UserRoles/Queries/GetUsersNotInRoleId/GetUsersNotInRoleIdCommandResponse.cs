namespace AgendaManager.Application.Users.UserRoles.Queries.GetUsersNotInRoleId;

public record GetUsersNotInRoleIdCommandResponse(Guid UserId, string Email);
