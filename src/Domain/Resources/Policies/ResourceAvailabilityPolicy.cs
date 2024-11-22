using AgendaManager.Domain.Calendars.Configurations;
using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.Errors;
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
        List<CalendarConfiguration> configurations,
        CancellationToken cancellationToken = default)
    {
        var configuration = configurations.FirstOrDefault(
            cc => cc.Category == CalendarConfigurationKeys.ResourcesSchedules.AvailabilityStrategy);

        if (configuration == null)
        {
            return CalendarConfigurationErrors.KeyNotFound;
        }

        // If not required validate availability, return success.
        if (configuration.SelectedKey is CalendarConfigurationKeys.ResourcesSchedules.AvailabilityOptions
                .IgnoreSchedules)
        {
            return Result.Success();
        }

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
