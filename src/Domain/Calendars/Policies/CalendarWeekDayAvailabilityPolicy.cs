using AgendaManager.Domain.Calendars.Errors;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;

namespace AgendaManager.Domain.Calendars.Policies;

public class CalendarWeekDayAvailabilityPolicy : ICalendarWeekDayAvailabilityPolicy
{
    public Result IsAvailable(Calendar calendar, Period period)
    {
        if (!calendar.IsAvailableDay(period.Start.DayOfWeek) || !calendar.IsAvailableDay(period.End.DayOfWeek))
        {
            return CalendarErrors.WeekDayNotAvailable;
        }

        return Result.Success();
    }
}
