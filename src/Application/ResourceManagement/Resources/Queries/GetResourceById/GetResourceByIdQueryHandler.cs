using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.ResourceManagement.Resources.Errors;
using AgendaManager.Domain.ResourceManagement.Resources.Interfaces;
using AgendaManager.Domain.ResourceManagement.Resources.ValueObjects;

namespace AgendaManager.Application.ResourceManagement.Resources.Queries.GetResourceById;

internal class GetResourceByIdQueryHandler(IResourceRepository resourceRepository)
    : IQueryHandler<GetResourceByIdQuery, GetResourceByIdQueryResponse>
{
    public async Task<Result<GetResourceByIdQueryResponse>> Handle(
        GetResourceByIdQuery request,
        CancellationToken cancellationToken)
    {
        // Get resource by id and check if it exists.
        var resource = await resourceRepository.GetByIdAsync(ResourceId.From(request.ResourceId), cancellationToken);
        if (resource == null)
        {
            return ResourceErrors.NotFound;
        }

        // Map resource to response.
        var response = new GetResourceByIdQueryResponse(
            ResourceId: resource.Id.Value,
            Name: resource.Name,
            Description: resource.Description,
            TextColor: resource.ColorScheme.Text,
            BackgroundColor: resource.ColorScheme.Background,
            IsActive: resource.IsActive,
            DeactivationReason: resource.DeactivationReason,
            CreatedAt: resource.CreatedAt);

        return Result.Success(response);
    }
}
