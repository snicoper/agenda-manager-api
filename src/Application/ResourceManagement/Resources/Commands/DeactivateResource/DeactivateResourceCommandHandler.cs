using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.ResourceManagement.Resources.Errors;
using AgendaManager.Domain.ResourceManagement.Resources.Interfaces;
using AgendaManager.Domain.ResourceManagement.Resources.ValueObjects;

namespace AgendaManager.Application.ResourceManagement.Resources.Commands.DeactivateResource;

internal class DeactivateResourceCommandHandler(IResourceRepository resourceRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<DeactivateResourceCommand>
{
    public async Task<Result> Handle(DeactivateResourceCommand request, CancellationToken cancellationToken)
    {
        // Get resource and check if it exists.
        var resource = await resourceRepository.GetByIdAsync(ResourceId.From(request.ResourceId), cancellationToken);
        if (resource is null)
        {
            return ResourceErrors.NotFound;
        }

        // Deactivate resource.
        resource.Deactivate(request.DeactivationReason);

        // Update resource and save changes.
        resourceRepository.Update(resource);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.NoContent();
    }
}
