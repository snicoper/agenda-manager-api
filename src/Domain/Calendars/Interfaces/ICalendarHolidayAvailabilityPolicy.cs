using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;

namespace AgendaManager.Domain.Calendars.Interfaces;

public interface ICalendarHolidayAvailabilityPolicy
{
    Task<Result> IsAvailableAsync(CalendarId calendarId, Period period, CancellationToken cancellationToken);
}
