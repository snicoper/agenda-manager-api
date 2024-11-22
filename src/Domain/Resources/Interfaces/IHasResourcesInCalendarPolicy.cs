using AgendaManager.Domain.Calendars.ValueObjects;

namespace AgendaManager.Domain.Resources.Interfaces;

public interface IHasResourcesInCalendarPolicy
{
    Task<bool> IsSatisfiedByAsync(CalendarId calendarId, CancellationToken cancellationToken = default);
}
