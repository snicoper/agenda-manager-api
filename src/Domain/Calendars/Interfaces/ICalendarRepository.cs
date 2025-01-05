using AgendaManager.Domain.Calendars.ValueObjects;

namespace AgendaManager.Domain.Calendars.Interfaces;

public interface ICalendarRepository
{
    IQueryable<Calendar> GetQueryable();

    Task<List<Calendar>> GetCalendarsAsync(CancellationToken cancellationToken = default);

    Task<Calendar?> GetByIdAsync(CalendarId id, CancellationToken cancellationToken = default);

    Task<Calendar?> GetByIdWithSettingsAsync(CalendarId calendarId, CancellationToken cancellationToken = default);

    Task<Calendar?> GetByIdWithHolidaysAsync(
        CalendarId calendarId,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByCalendarIdAsync(CalendarId calendarId, CancellationToken cancellationToken = default);

    Task<bool> ExistsByNameAsync(CalendarId calendarId, string name, CancellationToken cancellationToken = default);

    Task AddAsync(Calendar calendar, CancellationToken cancellationToken = default);

    void Update(Calendar calendar);

    void Delete(Calendar calendar);
}
