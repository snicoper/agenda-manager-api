namespace AgendaManager.Application.Calendars.Queries.GetCalendarHolidays;

public record GetCalendarHolidaysQueryResponse(
    Guid CalendarHolidayId,
    string Name,
    DateTimeOffset Start,
    DateTimeOffset End);
