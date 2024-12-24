namespace AgendaManager.Application.Calendars.Queries.GetCalendarById;

public record GetCalendarByIdQueryResponse(
    Guid CalendarId,
    string Name,
    string Description,
    bool IsActive,
    DateTimeOffset CreatedAt);
