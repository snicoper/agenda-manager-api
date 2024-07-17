namespace AgendaManager.Application.Users.Queries.GetUsers;

public record GetUsersQueryResponse(Guid Id, string Email, string? FirstName, string? LastName);
