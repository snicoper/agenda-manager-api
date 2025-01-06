namespace AgendaManager.WebApi.Controllers.Users.Accounts.Contracts;

public record FilterAccountsRequest(string Term, int PageSize);
