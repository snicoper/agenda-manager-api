namespace AgendaManager.Application.Users.Accounts.FilterAccounts;

public record FilterAccountsQueryResponse(Guid UserId, string Email, string FirstName, string LastName);
