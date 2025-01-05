using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;

namespace AgendaManager.Domain.Calendars.Interfaces;

public interface ICalendarHolidayAvailabilityExcludeSelfPolicy
{
    Task<Result> IsAvailableAsync(
        CalendarId calendarId,
        CalendarHolidayId calendarHolidayId,
        Period period,
        CancellationToken cancellationToken);
}
