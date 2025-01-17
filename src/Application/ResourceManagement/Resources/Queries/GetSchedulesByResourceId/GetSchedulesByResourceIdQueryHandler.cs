using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Users.Services;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.ResourceManagement.Resources.Interfaces;
using AgendaManager.Domain.ResourceManagement.Resources.ValueObjects;

namespace AgendaManager.Application.ResourceManagement.Resources.Queries.GetSchedulesByResourceId;

internal class GetSchedulesByResourceIdQueryHandler(
    IResourceRepository resourceRepository,
    ICurrentUserProvider currentUserProvider)
    : IQueryHandler<GetSchedulesByResourceIdQuery, ICollection<GetSchedulesByResourceIdQueryResponse>>
{
    public async Task<Result<ICollection<GetSchedulesByResourceIdQueryResponse>>> Handle(
        GetSchedulesByResourceIdQuery request,
        CancellationToken cancellationToken)
    {
        // Get schedules by resource id and calendar id selected.
        var schedules = await resourceRepository.GetSchedulesByResourceIdAsync(
            ResourceId.From(request.ResourceId),
            currentUserProvider.GetSelectedCalendarId(),
            cancellationToken);

        // Map schedules to response.
        ICollection<GetSchedulesByResourceIdQueryResponse> response = schedules.Select(
                s => new GetSchedulesByResourceIdQueryResponse(
                    ScheduleId: s.CalendarId.Value,
                    ResourceId: s.CalendarId.Value,
                    Name: s.Name,
                    Description: s.Description,
                    IsActive: s.IsActive,
                    Type: s.Type,
                    AvailableDays: s.AvailableDays,
                    Start: s.Period.Start,
                    End: s.Period.End))
            .ToList();

        return Result.Success(response);
    }
}
