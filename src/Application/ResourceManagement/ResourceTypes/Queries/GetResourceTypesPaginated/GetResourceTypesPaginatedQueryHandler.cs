using AgendaManager.Application.Common.Http;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.ResourceManagement.Resources.Interfaces;

namespace AgendaManager.Application.ResourceManagement.ResourceTypes.Queries.GetResourceTypesPaginated;

internal class GetResourceTypesPaginatedQueryHandler(IResourceRepository resourceRepository)
    : IQueryHandler<GetResourceTypesPaginatedQuery,
        ResponseData<GetResourceTypesPaginatedQueryResponse>>
{
    public async Task<Result<ResponseData<GetResourceTypesPaginatedQueryResponse>>> Handle(
        GetResourceTypesPaginatedQuery request,
        CancellationToken cancellationToken)
    {
        // 1. Get the resource types queryable from the repository.
        var resourceTypes = resourceRepository.GetQueryable()
            .AsQueryable();

        // 2. Create the response data.
        var responseData = await ResponseData<GetResourceTypesPaginatedQueryResponse>.CreateAsync(
            source: resourceTypes,
            projection: rt => new GetResourceTypesPaginatedQueryResponse(
                ResourceTypeId: rt.Id.Value,
                Name: rt.Name,
                Description: rt.Description,
                Category: rt.Category),
            request: request.RequestData,
            cancellationToken: cancellationToken);

        // 3. Return the response data.
        return Result.Success(responseData);
    }
}
