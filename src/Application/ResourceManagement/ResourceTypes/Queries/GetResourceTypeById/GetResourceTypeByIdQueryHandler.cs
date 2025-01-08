using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.Errors;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.Interfaces;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.ValueObjects;

namespace AgendaManager.Application.ResourceManagement.ResourceTypes.Queries.GetResourceTypeById;

internal class GetResourceTypeByIdQueryHandler(IResourceTypeRepository resourceTypeRepository)
    : IQueryHandler<GetResourceTypeByIdQuery, GetResourceTypeByIdQueryResponse>
{
    public async Task<Result<GetResourceTypeByIdQueryResponse>> Handle(
        GetResourceTypeByIdQuery request,
        CancellationToken cancellationToken)
    {
        // Get the resource type by id and check if exists.
        var resourceType = await resourceTypeRepository.GetByIdAsync(
            ResourceTypeId.From(request.ResourceTypeId),
            cancellationToken);

        if (resourceType is null)
        {
            return ResourceTypeErrors.ResourceTypeNotFound;
        }

        // Map the resource type to response.
        var response = new GetResourceTypeByIdQueryResponse(
            resourceType.Id.Value,
            resourceType.Name,
            resourceType.Description,
            resourceType.Category);

        return Result.Success(response);
    }
}
