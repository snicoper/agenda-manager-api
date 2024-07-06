namespace AgendaManager.Application.Users.Queries.GetUsers;

public record GetUsersResponse(Guid Id, string UserName, string Email, string? FirstName, string? LastName);
