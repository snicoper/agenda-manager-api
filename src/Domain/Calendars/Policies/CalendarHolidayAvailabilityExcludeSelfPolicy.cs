using AgendaManager.Domain.Calendars.Errors;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;

namespace AgendaManager.Domain.Calendars.Policies;

public class CalendarHolidayAvailabilityExcludeSelfPolicy(ICalendarHolidayRepository holidayRepository)
    : ICalendarHolidayAvailabilityExcludeSelfPolicy
{
    public async Task<Result> IsAvailableAsync(
        CalendarId calendarId,
        CalendarHolidayId calendarHolidayId,
        Period period,
        CancellationToken cancellationToken)
    {
        var isOverlapping = await holidayRepository.IsOverlappingInPeriodByCalendarIdExcludeSelfAsync(
            calendarId: calendarId,
            calendarHolidayId: calendarHolidayId,
            period: period,
            cancellationToken: cancellationToken);

        return isOverlapping ? CalendarHolidayErrors.HolidaysOverlap : Result.Success();
    }
}
