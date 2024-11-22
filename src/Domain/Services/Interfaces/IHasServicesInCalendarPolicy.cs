using AgendaManager.Domain.Calendars.ValueObjects;

namespace AgendaManager.Domain.Services.Interfaces;

public interface IHasServicesInCalendarPolicy
{
    Task<bool> IsSatisfiedByAsync(CalendarId calendarId, CancellationToken cancellationToken = default);
}
