using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;

namespace AgendaManager.Domain.Calendars.Interfaces;

public interface ICalendarWeekDayAvailabilityPolicy
{
    Result IsAvailable(Calendar calendar, Period period);
}
