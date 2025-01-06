using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.Users.Accounts.FilterAccounts;

[Authorize(Permissions = SystemPermissions.Users.Read)]
public record FilterAccountsQuery(string Term, int PageSize) : IQuery<List<FilterAccountsQueryResponse>>;
