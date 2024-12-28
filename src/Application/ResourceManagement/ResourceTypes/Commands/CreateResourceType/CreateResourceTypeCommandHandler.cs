using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.Services;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.ValueObjects;
using MediatR;

namespace AgendaManager.Application.ResourceManagement.ResourceTypes.Commands.CreateResourceType;

internal class CreateResourceTypeCommandHandler(ResourceTypeManager resourceTypeManager, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateResourceTypeCommand, Result>
{
    public async Task<Result> Handle(CreateResourceTypeCommand request, CancellationToken cancellationToken)
    {
        // 1. Create a new resource type.
        var createResult = await resourceTypeManager.CreateResourceTypeAsync(
            resourceTypeId: ResourceTypeId.Create(),
            name: request.Name,
            description: request.Description,
            category: request.Category,
            cancellationToken: cancellationToken);

        // 2. Save changes.
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // 3. Return the result.
        return createResult.IsFailure ? createResult : Result.Create();
    }
}
