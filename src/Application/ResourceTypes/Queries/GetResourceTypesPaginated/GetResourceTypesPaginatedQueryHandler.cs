using AgendaManager.Application.Common.Http;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Application.ResourceTypes.Queries.GetResourceTypesPaginated;

internal class GetResourceTypesPaginatedQueryHandler : IQueryHandler<GetResourceTypesPaginatedQuery,
    ResponseData<GetResourceTypesPaginatedQueryResponse>>
{
    public Task<Result<ResponseData<GetResourceTypesPaginatedQueryResponse>>> Handle(
        GetResourceTypesPaginatedQuery request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
