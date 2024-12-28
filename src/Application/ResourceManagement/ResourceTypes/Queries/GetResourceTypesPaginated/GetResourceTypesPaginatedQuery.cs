using AgendaManager.Application.Common.Http;
using AgendaManager.Application.Common.Interfaces.Messaging;

namespace AgendaManager.Application.ResourceManagement.ResourceTypes.Queries.GetResourceTypesPaginated;

public record GetResourceTypesPaginatedQuery(RequestData RequestData)
    : IQuery<ResponseData<GetResourceTypesPaginatedQueryResponse>>;
