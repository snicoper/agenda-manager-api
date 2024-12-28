using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.ResourceManagement.Resources.Interfaces;

namespace AgendaManager.Domain.ResourceManagement.Resources.Policies;

public class HasResourcesInCalendarPolicy(IResourceRepository resourceRepository) : IHasResourcesInCalendarPolicy
{
    public async Task<bool> IsSatisfiedByAsync(CalendarId calendarId, CancellationToken cancellationToken = default)
    {
        var hasResources = await resourceRepository.HasResourcesInCalendarAsync(calendarId, cancellationToken);

        return hasResources;
    }
}
