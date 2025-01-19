using AgendaManager.Application.Common.Http;
using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.ResourceManagement.Resources.Services;
using AgendaManager.Application.Users.Services;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.ResourceManagement.Resources.Interfaces;
using AgendaManager.Domain.ResourceManagement.Resources.ValueObjects;

namespace AgendaManager.Application.ResourceManagement.Resources.Queries.GetSchedulesByResourceIdPaginated;

internal class GetSchedulesByResourceIdPaginatedQueryHandler(
    IResourceScheduleRepository resourceScheduleRepository,
    ICurrentUserProvider currentUserProvider)
    : IQueryHandler<
        GetSchedulesByResourceIdPaginatedQuery,
        ResponseData<GetSchedulesByResourceIdPaginatedQueryResponse>>
{
    public async Task<Result<ResponseData<GetSchedulesByResourceIdPaginatedQueryResponse>>> Handle(
        GetSchedulesByResourceIdPaginatedQuery request,
        CancellationToken cancellationToken)
    {
        // Get schedules by resource id and calendar id selected.
        var schedules = resourceScheduleRepository.GetQueryable()
            .Where(
                r => r.CalendarId == currentUserProvider.GetSelectedCalendarId() &&
                    r.ResourceId == ResourceId.From(request.ResourceId));

        // Apply filters to the schedules queryable.
        schedules = ResourceScheduleFilter.ApplyFilters(schedules, request.RequestData);

        // Create the response data.
        var responseData = await ResponseData<GetSchedulesByResourceIdPaginatedQueryResponse>.CreateAsync(
            source: schedules,
            projection: s => new GetSchedulesByResourceIdPaginatedQueryResponse(
                ScheduleId: s.CalendarId.Value,
                ResourceId: s.CalendarId.Value,
                Name: s.Name,
                Description: s.Description,
                IsActive: s.IsActive,
                Type: s.Type,
                AvailableDays: s.AvailableDays,
                Start: s.Period.Start,
                End: s.Period.End),
            request: request.RequestData,
            cancellationToken: cancellationToken);

        return Result.Success(responseData);
    }
}
