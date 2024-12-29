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
        // 1. Create a new resource type.
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

        // 2. Save changes.
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // 3. Return the result.
        var response = new CreateResourceTypeCommandResponse(createResult.Value?.Id.Value ?? Guid.Empty);

        return Result.Create(response);
    }
}
