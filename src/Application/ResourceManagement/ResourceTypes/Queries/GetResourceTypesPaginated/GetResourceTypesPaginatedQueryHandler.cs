using AgendaManager.Application.Common.Http;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.Interfaces;

namespace AgendaManager.Application.ResourceManagement.ResourceTypes.Queries.GetResourceTypesPaginated;

internal class GetResourceTypesPaginatedQueryHandler(IResourceTypeRepository resourceTypeRepository)
    : IQueryHandler<GetResourceTypesPaginatedQuery,
        ResponseData<GetResourceTypesPaginatedQueryResponse>>
{
    public async Task<Result<ResponseData<GetResourceTypesPaginatedQueryResponse>>> Handle(
        GetResourceTypesPaginatedQuery request,
        CancellationToken cancellationToken)
    {
        // Get the resource types queryable from the repository.
        var resourceTypes = resourceTypeRepository.GetQueryable()
            .AsQueryable();

        // Create the response data.
        var responseData = await ResponseData<GetResourceTypesPaginatedQueryResponse>.CreateAsync(
            source: resourceTypes,
            projection: rt => new GetResourceTypesPaginatedQueryResponse(
                ResourceTypeId: rt.Id.Value,
                Name: rt.Name,
                Description: rt.Description,
                Category: rt.Category),
            request: request.RequestData,
            cancellationToken: cancellationToken);

        // Return the response data.
        return Result.Success(responseData);
    }
}
