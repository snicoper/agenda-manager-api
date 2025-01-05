namespace AgendaManager.WebApi.Controllers.Calendars.Contracts;

public record CalendarHolidayUpdateRequest(string Name, DateTimeOffset Start, DateTimeOffset End);
