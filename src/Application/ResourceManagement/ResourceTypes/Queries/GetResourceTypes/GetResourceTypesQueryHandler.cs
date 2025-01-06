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
        var resourceTypes = await resourceTypeRepository.GetQueryable().ToListAsync(cancellationToken);

        var response = resourceTypes.Select(x => new GetResourceTypesQueryResponse(x.Id.Value, x.Name)).ToList();

        return Result.Success(response);
    }
}
