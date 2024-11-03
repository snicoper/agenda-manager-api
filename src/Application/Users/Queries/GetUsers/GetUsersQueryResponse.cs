namespace AgendaManager.Application.Users.Queries.GetUsers;

public record GetUsersQueryResponse(Guid UserId, string Email, string? FirstName, string? LastName);
