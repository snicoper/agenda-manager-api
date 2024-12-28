using AgendaManager.Application.Common.Authorization;
using AgendaManager.Application.Common.Http;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Authorization.Constants;

namespace AgendaManager.Application.ResourceManagement.ResourceTypes.Queries.GetResourceTypesPaginated;

[Authorize(Permissions = SystemPermissions.ResourceTypes.Read)]
public record GetResourceTypesPaginatedQuery(RequestData RequestData)
    : IQuery<ResponseData<GetResourceTypesPaginatedQueryResponse>>;
