using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Calendars.Errors;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Application.Calendars.Queries.GetCalendarHolidaysInYear;

internal class GetCalendarHolidaysInYearHandler(ICalendarRepository calendarRepository)
    : IQueryHandler<GetCalendarHolidaysInYearQuery, List<GetCalendarHolidaysInYearQueryResponse>>
{
    public async Task<Result<List<GetCalendarHolidaysInYearQueryResponse>>> Handle(
        GetCalendarHolidaysInYearQuery request,
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

        // Get the holidays in the year.
        var holidays = calendar.Holidays
            .Where(holiday => holiday.Period.Start.Year == request.Year || holiday.Period.End.Year == request.Year);

        // Map to response.
        var response = holidays.Select(
            holiday => new GetCalendarHolidaysInYearQueryResponse(
                CalendarHolidayId: holiday.Id.Value,
                Name: holiday.Name,
                Start: holiday.Period.Start,
                End: holiday.Period.End)).ToList();

        return Result.Success(response);
    }
}
