using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Interfaces.Persistence;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.ResourceManagement.Resources.Entities;
using AgendaManager.Domain.ResourceManagement.Resources.Errors;
using AgendaManager.Domain.ResourceManagement.Resources.Interfaces;
using AgendaManager.Domain.ResourceManagement.Resources.ValueObjects;

namespace AgendaManager.Application.ResourceManagement.Resources.Commands.CreateSchedule;

internal class CreateScheduleCommandHandler(IResourceRepository resourceRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<CreateScheduleCommand, CreateScheduleCommandResponse>
{
    public async Task<Result<CreateScheduleCommandResponse>> Handle(
        CreateScheduleCommand request,
        CancellationToken cancellationToken)
    {
        // Get the resource and check if it exists.
        var resource = await resourceRepository.GetByIdAsync(ResourceId.From(request.ResourceId), cancellationToken);
        if (resource is null)
        {
            return ResourceErrors.NotFound;
        }

        // Create the schedule.
        var schedule = ResourceSchedule.Create(
            resourceScheduleId: ResourceScheduleId.Create(),
            resourceId: ResourceId.From(request.ResourceId),
            calendarId: resource.CalendarId,
            period: Period.From(request.Start, request.End),
            type: request.Type,
            availableDays: request.AvailableDays,
            name: request.Name,
            description: request.Description,
            isActive: true);

        // Add the schedule to the resource.
        resource.AddSchedule(schedule);

        // Save the changes.
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // Map to response and return result.
        var response = new CreateScheduleCommandResponse(schedule.Id.Value);

        return Result.Success(response);
    }
}
