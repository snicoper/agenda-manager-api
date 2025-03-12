using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.ResourceManagement.Resources.Errors;
using AgendaManager.Domain.ResourceManagement.Resources.Interfaces;
using AgendaManager.Domain.ResourceManagement.Resources.ValueObjects;

namespace AgendaManager.Application.ResourceManagement.Resources.Commands.ActivateResource;

internal class ActivateResourceCommandHandler(IResourceRepository resourceRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<ActivateResourceCommand>
{
    public async Task<Result> Handle(ActivateResourceCommand request, CancellationToken cancellationToken)
    {
        // Get the resource and check if exists.
        var resource = await resourceRepository.GetByIdAsync(ResourceId.From(request.ResourceId), cancellationToken);
        if (resource is null)
        {
            return ResourceErrors.NotFound;
        }

        // Activate the resource.
        resource.Activate();

        // Update the resource and save changes.
        resourceRepository.Update(resource);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.NoContent();
    }
}
