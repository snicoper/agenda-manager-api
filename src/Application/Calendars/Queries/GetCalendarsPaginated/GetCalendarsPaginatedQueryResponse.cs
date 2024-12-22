namespace AgendaManager.Application.Calendars.Queries.GetCalendarsPaginated;

public record GetCalendarsPaginatedQueryResponse(
    Guid CalendarId,
    string Name,
    string Description,
    bool IsActive);
