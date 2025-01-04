using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Calendars.Errors;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Application.Calendars.Queries.GetCalendarHolidayById;

internal class GetCalendarHolidayByIdHandler(ICalendarRepository calendarRepository)
    : IQueryHandler<GetCalendarHolidayByIdQuery, GetCalendarHolidayByIdQueryResponse>
{
    public async Task<Result<GetCalendarHolidayByIdQueryResponse>> Handle(
        GetCalendarHolidayByIdQuery request,
        CancellationToken cancellationToken)
    {
        // Get the calendar by id and check if exists.
        var calendar = await calendarRepository.GetByIdWithHolidaysAsync(
            CalendarId.From(request.CalendarId),
            cancellationToken);

        if (calendar == null)
        {
            return CalendarErrors.CalendarNotFound;
        }

        // Get the holiday by id and check if exists.
        var holiday = calendar.Holidays
            .FirstOrDefault(holiday => holiday.Id == CalendarHolidayId.From(request.CalendarHolidayId));

        if (holiday is null)
        {
            return CalendarHolidayErrors.CalendarHolidayNotFound;
        }

        // Map to response.
        var response = new GetCalendarHolidayByIdQueryResponse(
            CalendarHolidayId: holiday.Id.Value,
            Name: holiday.Name,
            Start: holiday.Period.Start,
            End: holiday.Period.End);

        return Result.Success(response);
    }
}
