using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.Services;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.ValueObjects;

namespace AgendaManager.Application.ResourceManagement.ResourceTypes.Commands.UpdateResourceType;

internal class UpdateResourceTypeCommandHandler(ResourceTypeManager resourceTypeManager, IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateResourceTypeCommand>
{
    public async Task<Result> Handle(UpdateResourceTypeCommand request, CancellationToken cancellationToken)
    {
        // 1. Update resource type.
        var result = await resourceTypeManager.UpdateResourceTypeAsync(
            ResourceTypeId.From(request.ResourceTypeId),
            request.Name,
            request.Description,
            cancellationToken);

        // 2. Save changes.
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return result.IsFailure ? result : Result.NoContent();
    }
}
