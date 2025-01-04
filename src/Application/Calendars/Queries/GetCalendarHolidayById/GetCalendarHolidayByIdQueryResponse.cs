namespace AgendaManager.Application.Calendars.Queries.GetCalendarHolidayById;

public record GetCalendarHolidayByIdQueryResponse(
    Guid CalendarHolidayId,
    string Name,
    DateTimeOffset Start,
    DateTimeOffset End);
