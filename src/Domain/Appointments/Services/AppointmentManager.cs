using AgendaManager.Domain.Appointments.Interfaces;
using AgendaManager.Domain.Appointments.ValueObjects;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects.Period;
using AgendaManager.Domain.Resources;
using AgendaManager.Domain.Resources.Interfaces;
using AgendaManager.Domain.Services.Interfaces;
using AgendaManager.Domain.Services.ValueObjects;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Appointments.Services;

public class AppointmentManager(
    ICalendarConfigurationRepository configurationRepository,
    ICalendarHolidayAvailabilityPolicy calendarHolidayPolicy,
    IAppointmentCreationStrategyPolicy creationStrategyPolicy,
    IAppointmentOverlapPolicy overlapPolicy,
    IResourceAvailabilityPolicy resourcePolicy,
    IServiceRequirementsPolicy servicePolicy)
{
    public async Task<Result<Appointment>> CreateAppointmentAsync(
        CalendarId calendarId,
        ServiceId serviceId,
        UserId userId,
        Period period,
        List<Resource> resources,
        CancellationToken cancellationToken)
    {
        // 1. Get calendar configurations.
        var configurations = await configurationRepository
            .GetConfigurationsByCalendarIdAsync(calendarId, cancellationToken);

        // 2. Validate calendar and holidays.
        var holidayResult = await calendarHolidayPolicy.IsAvailableAsync(calendarId, period, cancellationToken);

        if (holidayResult.IsFailure)
        {
            return holidayResult.MapToValue<Appointment>();
        }

        // 3. Determine initial state based on creation strategy.
        var statusResult = creationStrategyPolicy.DetermineInitialStatus(configurations);

        // 4. Validate appointment overlapping if required.
        var overlapResult = await overlapPolicy.IsOverlappingAsync(
            calendarId,
            period,
            configurations,
            cancellationToken);

        if (overlapResult.IsFailure)
        {
            return overlapResult.MapToValue<Appointment>();
        }

        // 5. Validate resource availability.
        var resourceResult = await resourcePolicy.IsAvailableAsync(calendarId, period, cancellationToken);

        if (resourceResult.IsFailure)
        {
            return resourceResult.MapToValue<Appointment>();
        }

        // 6. Validate service requirements.
        var serviceResult = await servicePolicy.IsSatisfiedByAsync(serviceId, resources, cancellationToken);

        if (serviceResult.IsFailure)
        {
            return serviceResult.MapToValue<Appointment>();
        }

        // 7. Create appointment.
        var appointment = Appointment.Create(
            id: AppointmentId.Create(),
            calendarId: calendarId,
            serviceId: serviceId,
            userId: userId,
            period: period,
            status: statusResult.Value,
            resources: resources);

        return appointment;
    }
}
