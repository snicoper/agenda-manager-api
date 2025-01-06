using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Http;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.Resources.Queries.GetResourcesPaginated;

[Authorize(Permissions = SystemPermissions.Resources.Read)]
public record GetResourcesPaginatedQuery(RequestData RequestData)
    : IQuery<ResponseData<GetResourcesPaginatedQueryResponse>>;
