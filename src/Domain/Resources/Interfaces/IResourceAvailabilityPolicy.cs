using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects.Period;

namespace AgendaManager.Domain.Resources.Interfaces;

public interface IResourceAvailabilityPolicy
{
    Task<Result> IsAvailableAsync(CalendarId calendarId, Period period, CancellationToken cancellationToken = default);
}
