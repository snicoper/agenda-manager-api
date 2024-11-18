using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects.Period;
using AgendaManager.Domain.Resources.Interfaces;

namespace AgendaManager.Domain.Resources.Policies;

public class ResourceAvailabilityPolicy : IResourceAvailabilityPolicy
{
    public Task<Result> IsAvailableAsync(
        CalendarId calendarId,
        Period period,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
