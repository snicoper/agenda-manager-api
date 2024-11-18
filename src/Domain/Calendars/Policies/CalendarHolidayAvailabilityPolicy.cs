using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects.Period;

namespace AgendaManager.Domain.Calendars.Policies;

public class CalendarHolidayAvailabilityPolicy : ICalendarHolidayAvailabilityPolicy
{
    public Task<Result> ValidateAsync(CalendarId calendarId, Period period, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
