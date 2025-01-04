namespace AgendaManager.Application.Calendars.Queries.GetCalendarHolidaysInYear;

public record GetCalendarHolidaysInYearQueryResponse(
    Guid CalendarHolidayId,
    string Name,
    DateTimeOffset Start,
    DateTimeOffset End);
