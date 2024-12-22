namespace AgendaManager.WebApi.Controllers.Calendars.Contracts;

public record CreateCalendarRequest(string Name, string Description, string IanaTimeZone);
