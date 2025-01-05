using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.ValueObjects;

namespace AgendaManager.Domain.Calendars.Interfaces;

public interface ICalendarHolidayRepository
{
    Task<bool> IsOverlappingInPeriodByCalendarIdAsync(
        CalendarId calendarId,
        Period period,
        CancellationToken cancellationToken = default);

    Task<bool> IsOverlappingInPeriodByCalendarIdExcludeSelfAsync(
        CalendarId calendarId,
        CalendarHolidayId calendarHolidayId,
        Period period,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsHolidayNameAsync(
        CalendarId calendarId,
        CalendarHolidayId calendarHolidayId,
        string name,
        CancellationToken cancellationToken = default);
}
