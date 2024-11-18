using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects.Period;

namespace AgendaManager.Domain.Calendars.Interfaces;

public interface ICalendarHolidayAvailabilityPolicy
{
    Task<Result> ValidateAsync(CalendarId calendarId, Period period, CancellationToken cancellationToken);
}
