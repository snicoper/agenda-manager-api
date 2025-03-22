using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Calendars.Errors;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Application.Calendars.Queries.GetCalendarById;

internal class GetCalendarByIdQueryHandler(ICalendarRepository calendarRepository)
    : IQueryHandler<GetCalendarByIdQuery, GetCalendarByIdQueryResponse>
{
    public async Task<Result<GetCalendarByIdQueryResponse>> Handle(
        GetCalendarByIdQuery request,
        CancellationToken cancellationToken)
    {
        // Get the calendar by id and check if exists.
        var calendar = await calendarRepository.GetByIdAsync(CalendarId.From(request.CalendarId), cancellationToken);
        if (calendar is null)
        {
            return CalendarErrors.CalendarNotFound;
        }

        // Map the calendar to the response.
        var response = new GetCalendarByIdQueryResponse(
            CalendarId: calendar.Id.Value,
            Name: calendar.Name,
            Description: calendar.Description,
            IsActive: calendar.IsActive,
            AvailableDays: calendar.AvailableDays,
            CreatedAt: calendar.CreatedAt);

        return Result.Success(response);
    }
}
