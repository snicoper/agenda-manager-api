using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Domain.Calendars.Errors;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Application.Calendars.Queries.GetCalendarHolidays;

internal class GetCalendarHolidaysQueryHandler
    : IQueryHandler<GetCalendarHolidaysQuery, List<GetCalendarHolidaysQueryResponse>>
{
    private readonly ICalendarRepository _calendarRepository;

    public GetCalendarHolidaysQueryHandler(ICalendarRepository calendarRepository)
    {
        _calendarRepository = calendarRepository;
    }

    public async Task<Result<List<GetCalendarHolidaysQueryResponse>>> Handle(
        GetCalendarHolidaysQuery request,
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
            holiday => new GetCalendarHolidaysQueryResponse(
                CalendarHolidayId: holiday.Id.Value,
                Name: holiday.Name,
                Start: holiday.Period.Start,
                End: holiday.Period.End)).ToList();

        return Result.Success(response);
    }
}
