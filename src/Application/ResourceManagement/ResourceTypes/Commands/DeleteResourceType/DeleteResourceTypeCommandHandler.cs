using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.Services;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.ValueObjects;

namespace AgendaManager.Application.ResourceManagement.ResourceTypes.Commands.DeleteResourceType;

internal class DeleteResourceTypeCommandHandler(ResourceTypeManager resourceTypeManager, IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteResourceTypeCommand>
{
    public async Task<Result> Handle(DeleteResourceTypeCommand request, CancellationToken cancellationToken)
    {
        // Check if can delete resource type.
        var deleteResult = await resourceTypeManager.DeleteResourceTypeAsync(
            ResourceTypeId.From(request.ResourceTypeId),
            cancellationToken);

        if (deleteResult.IsFailure)
        {
            return deleteResult;
        }

        // Save changes.
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.NoContent();
    }
}
