using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.Services;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.ValueObjects;
using MediatR;

namespace AgendaManager.Application.ResourceManagement.ResourceTypes.Commands.CreateResourceType;

internal class CreateResourceTypeCommandHandler(ResourceTypeManager resourceTypeManager, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateResourceTypeCommand, Result<CreateResourceTypeCommandResponse>>
{
    public async Task<Result<CreateResourceTypeCommandResponse>> Handle(
        CreateResourceTypeCommand request,
        CancellationToken cancellationToken)
    {
        // Create a new resource type.
        var createResult = await resourceTypeManager.CreateResourceTypeAsync(
            resourceTypeId: ResourceTypeId.Create(),
            name: request.Name,
            description: request.Description,
            category: request.Category,
            cancellationToken: cancellationToken);

        if (createResult.IsFailure)
        {
            return createResult.MapTo<CreateResourceTypeCommandResponse>();
        }

        // Save changes.
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // Map the result to the response.
        var response = new CreateResourceTypeCommandResponse(createResult.Value?.Id.Value ?? Guid.Empty);

        return Result.Create(response);
    }
}
