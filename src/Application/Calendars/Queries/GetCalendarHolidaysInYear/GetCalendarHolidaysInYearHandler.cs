using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Calendars.Errors;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Application.Calendars.Queries.GetCalendarHolidaysInYear;

internal class GetCalendarHolidaysInYearHandler
    : IQueryHandler<GetCalendarHolidaysInYearQuery, List<GetCalendarHolidaysInYearQueryResponse>>
{
    private readonly ICalendarRepository _calendarRepository;

    public GetCalendarHolidaysInYearHandler(ICalendarRepository calendarRepository)
    {
        _calendarRepository = calendarRepository;
    }

    public async Task<Result<List<GetCalendarHolidaysInYearQueryResponse>>> Handle(
        GetCalendarHolidaysInYearQuery request,
        CancellationToken cancellationToken)
    {
        var calendar = await _calendarRepository.GetByIdWithHolidaysAsync(
            CalendarId.From(request.CalendarId),
            cancellationToken);

        if (calendar == null)
        {
            return CalendarErrors.CalendarNotFound;
        }

        var holidays = calendar.Holidays
            .Where(holiday => holiday.Period.Start.Year == request.Year || holiday.Period.End.Year == request.Year);

        var response = holidays.Select(
            holiday => new GetCalendarHolidaysInYearQueryResponse(
                CalendarHolidayId: holiday.Id.Value,
                Name: holiday.Name,
                Start: holiday.Period.Start,
                End: holiday.Period.End)).ToList();

        return Result.Success(response);
    }
}
