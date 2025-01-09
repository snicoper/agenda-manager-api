using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Application.Users.Services;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.ResourceManagement.Resources.Services;
using AgendaManager.Domain.ResourceManagement.Resources.ValueObjects;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.ValueObjects;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Application.ResourceManagement.Resources.Commands.CreateResource;

internal class CreateResourceCommandHandler(
    ResourceManager resourceManager,
    ICurrentUserProvider currentUserProvider,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateResourceCommand>
{
    public async Task<Result> Handle(CreateResourceCommand request, CancellationToken cancellationToken)
    {
        var calendarId = currentUserProvider.GetSelectedCalendarId();

        var result = await resourceManager.CreateResourceAsync(
            resourceId: ResourceId.From(request.ResourceTypeId),
            userId: request.UserId is not null ? UserId.From(request.UserId.Value) : null,
            calendarId: calendarId,
            typeId: ResourceTypeId.From(request.ResourceTypeId),
            name: request.Name,
            description: request.Description,
            colorScheme: ColorScheme.From(request.TextColor, request.BackgroundColor),
            isActive: true,
            cancellationToken: cancellationToken);

        if (result.IsFailure)
        {
            return result;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Create();
    }
}
