namespace AgendaManager.WebApi.Controllers.Calendars.Contracts;

public record CreateCalendarHolidayCommandRequest(
    DateTimeOffset Start,
    DateTimeOffset End,
    string Name);
