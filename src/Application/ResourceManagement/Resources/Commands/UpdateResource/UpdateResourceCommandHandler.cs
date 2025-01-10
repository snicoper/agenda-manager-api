using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.ResourceManagement.Resources.Services;
using AgendaManager.Domain.ResourceManagement.Resources.ValueObjects;

namespace AgendaManager.Application.ResourceManagement.Resources.Commands.UpdateResource;

internal class UpdateResourceCommandHandler(ResourceManager resourceManager, IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateResourceCommand>
{
    public async Task<Result> Handle(UpdateResourceCommand request, CancellationToken cancellationToken)
    {
        // Update resource.
        var result = await resourceManager.UpdateResourceAsync(
            resourceId: ResourceId.From(request.ResourceId),
            name: request.Name,
            description: request.Description,
            colorScheme: ColorScheme.From(request.TextColor, request.BackgroundColor),
            cancellationToken: cancellationToken);

        if (result.IsFailure)
        {
            return result;
        }

        // Save changes.
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Create();
    }
}
