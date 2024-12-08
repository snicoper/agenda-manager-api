namespace AgendaManager.Application.Users.Accounts.Queries.GetAccountsPaginated;

public record GetAccountsPaginatedQueryResponse(
    Guid UserId,
    string Email,
    string FirstName,
    string LastName,
    bool IsActive,
    bool IsEmailConfirmed,
    bool IsCollaborator);
