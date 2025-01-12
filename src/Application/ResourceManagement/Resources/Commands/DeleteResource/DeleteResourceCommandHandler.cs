using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.ResourceManagement.Resources.Services;
using AgendaManager.Domain.ResourceManagement.Resources.ValueObjects;

namespace AgendaManager.Application.ResourceManagement.Resources.Commands.DeleteResource;

internal class DeleteResourceCommandHandler(ResourceManager resourceManager, IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteResourceCommand>
{
    public async Task<Result> Handle(DeleteResourceCommand request, CancellationToken cancellationToken)
    {
        // Delete the resource and check result.
        var result = await resourceManager.DeleteResourceAsync(ResourceId.From(request.ResourceId), cancellationToken);
        if (result.IsFailure)
        {
            return result;
        }

        // Save changes.
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Create();
    }
}
