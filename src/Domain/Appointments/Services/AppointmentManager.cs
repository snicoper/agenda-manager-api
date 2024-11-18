using AgendaManager.Domain.Appointments.Interfaces;
using AgendaManager.Domain.Appointments.ValueObjects;
using AgendaManager.Domain.Calendars.Interfaces;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects.Period;
using AgendaManager.Domain.Resources;
using AgendaManager.Domain.Resources.Interfaces;
using AgendaManager.Domain.Services.ValueObjects;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Appointments.Services;

public class AppointmentManager(
    ICalendarConfigurationRepository calendarConfigurationRepository,
    ICalendarHolidayAvailabilityPolicy calendarHolidayPolicy,
    IAppointmentCreationStrategyPolicy creationStrategyPolicy,
    IAppointmentOverlapPolicy overlapPolicy,
    IResourceAvailabilityPolicy resourcePolicy)
{
    public async Task<Result<Appointment>> CreateAppointmentAsync(
        CalendarId calendarId,
        ServiceId serviceId,
        UserId userId,
        Period period,
        List<Resource> resources,
        CancellationToken cancellationToken)
    {
        // 1. Obtener configuración.
        var configurations = await calendarConfigurationRepository
            .GetConfigurationsByCalendarIdAsync(calendarId, cancellationToken);

        // 2. Validar calendario y festivos.
        var holidayResult = await calendarHolidayPolicy.ValidateAsync(calendarId, period, cancellationToken);

        if (holidayResult.IsFailure)
        {
            return holidayResult.MapToValue<Appointment>();
        }

        // 3. Determinar estado inicial según estrategia.
        var statusResult = creationStrategyPolicy.DetermineInitialStatus(configurations);

        if (statusResult.IsFailure)
        {
            return statusResult.MapToValue<Appointment>();
        }

        // 4. Validar solapamientos si es necesario.
        var overlapResult = await overlapPolicy.IsOverlapping(calendarId, period, configurations, cancellationToken);

        if (overlapResult.IsFailure)
        {
            return overlapResult.MapToValue<Appointment>();
        }

        // 5. Validar disponibilidad de recursos.
        var resourceResult = await resourcePolicy.IsAvailableAsync(calendarId, period, cancellationToken);

        if (resourceResult.IsFailure)
        {
            return resourceResult.MapToValue<Appointment>();
        }

        // 6. Crear cita.
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
