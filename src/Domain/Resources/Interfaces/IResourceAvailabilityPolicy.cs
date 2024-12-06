using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;

namespace AgendaManager.Domain.Resources.Interfaces;

public interface IResourceAvailabilityPolicy
{
    Task<Result> IsAvailableAsync(
        CalendarId calendarId,
        List<Resource> resources,
        Period period,
        List<CalendarConfiguration> configurations,
        CancellationToken cancellationToken = default);
}
