using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Http;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.Users.Accounts.Queries.GetAccountsPaginated;

[Authorize(Permissions = SystemPermissions.Users.Read)]
public record GetAccountsPaginatedQuery(RequestData RequestData)
    : IQuery<ResponseData<GetAccountsPaginatedQueryResponse>>;
