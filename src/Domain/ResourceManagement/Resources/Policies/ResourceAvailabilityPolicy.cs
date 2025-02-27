﻿using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.Enums;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.ResourceManagement.Resources.Errors;
using AgendaManager.Domain.ResourceManagement.Resources.Interfaces;

namespace AgendaManager.Domain.ResourceManagement.Resources.Policies;

public class ResourceAvailabilityPolicy(IResourceRepository resourceRepository)
    : IResourceAvailabilityPolicy
{
    public async Task<Result> IsAvailableAsync(
        Calendar calendar,
        List<Resource> resources,
        Period period,
        CancellationToken cancellationToken = default)
    {
        // If not required validate availability, return success.
        if (calendar.Settings.ResourceScheduleValidation != ResourceScheduleValidationStrategy.Validate)
        {
            return Result.Success();
        }

        var isAvailable = await resourceRepository.AreResourcesAvailableInPeriodAsync(
            calendarId: calendar.Id,
            resources: resources,
            period: period,
            cancellationToken: cancellationToken);

        var result = isAvailable
            ? Result.Success()
            : ResourceErrors.ResourceNotAvailable;

        return result;
    }
}
