using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;

namespace AgendaManager.Domain.Calendars.Policies;

public class CalendarHolidayAvailabilityPolicy(ICalendarHolidayRepository holidayRepository)
    : ICalendarHolidayAvailabilityPolicy
{
    public async Task<Result> IsAvailableAsync(
        CalendarId calendarId,
        Period period,
        CancellationToken cancellationToken)
    {
        var isOverlapping = await holidayRepository.IsOverlappingInPeriodByCalendarIdAsync(
            calendarId: calendarId,
            period: period,
            cancellationToken: cancellationToken);

        return isOverlapping.IsFailure ? isOverlapping : Result.Success();
    }
}
