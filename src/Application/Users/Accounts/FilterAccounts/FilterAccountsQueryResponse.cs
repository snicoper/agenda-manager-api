namespace AgendaManager.Application.Users.Accounts.FilterAccounts;

public record FilterAccountsQueryResponse(Guid AccountId, string Email, string FirstName, string LastName);
