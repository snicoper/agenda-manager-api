using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects.Period;
using AgendaManager.Domain.Resources.Errors;
using AgendaManager.Domain.Resources.Interfaces;

namespace AgendaManager.Domain.Resources.Policies;

public class ResourceAvailabilityPolicy(IResourceRepository resourceRepository)
    : IResourceAvailabilityPolicy
{
    public async Task<Result> IsAvailableAsync(
        CalendarId calendarId,
        List<Resource> resources,
        Period period,
        CancellationToken cancellationToken = default)
    {
        var isAvailable = await resourceRepository.AreResourcesAvailableInPeriodAsync(
            calendarId,
            resources,
            period,
            cancellationToken);

        var result = isAvailable
            ? Result.Success()
            : ResourceErrors.ResourceNotAvailable;

        return result;
    }
}
