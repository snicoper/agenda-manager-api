using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Application.ResourceManagement.ResourceTypes.Queries.GetResourceTypes;

internal class GetResourceTypesQueryHandler(IResourceTypeRepository resourceTypeRepository)
    : IQueryHandler<GetResourceTypesQuery, List<GetResourceTypesQueryResponse>>
{
    public async Task<Result<List<GetResourceTypesQueryResponse>>> Handle(
        GetResourceTypesQuery request,
        CancellationToken cancellationToken)
    {
        // Get all resource types.
        var resourceTypes = await resourceTypeRepository.GetQueryable().ToListAsync(cancellationToken);

        // Map to response.
        var response = resourceTypes
            .Select(rt => new GetResourceTypesQueryResponse(rt.Id.Value, rt.Name, rt.Category))
            .ToList();

        return Result.Success(response);
    }
}
