using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Resources.Interfaces;

namespace AgendaManager.Domain.Resources.Policies;

public class HasResourcesInCalendarPolicy(IResourceRepository resourceRepository) : IHasResourcesInCalendarPolicy
{
    public async Task<bool> IsSatisfiedByAsync(CalendarId calendarId, CancellationToken cancellationToken = default)
    {
        var hasResources = await resourceRepository.HasResourcesInCalendarAsync(calendarId, cancellationToken);

        return hasResources;
    }
}
