using AgendaManager.Domain.Appointments.Interfaces;
using AgendaManager.Domain.Calendars.Configurations;
using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.Errors;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects.Period;

namespace AgendaManager.Domain.Appointments.Policies;

public class AppointmentOverlapPolicy(IAppointmentRepository appointmentRepository)
    : IAppointmentOverlapPolicy
{
    public async Task<Result> IsOverlappingAsync(
        CalendarId calendarId,
        Period period,
        List<CalendarConfiguration> configurations,
        CancellationToken cancellationToken = default)
    {
        // 1. Get calendar configurations.
        var configuration = configurations.FirstOrDefault(
            cc => cc.Category == CalendarConfigurationKeys.Appointments.OverlappingStrategy);

        if (configuration is null)
        {
            return CalendarConfigurationErrors.KeyNotFound;
        }

        // 2. Allow overlapping.
        if (configuration.SelectedKey is CalendarConfigurationKeys.Appointments.OverlappingOptions.AllowOverlapping)
        {
            return Result.Success();
        }

        // 3. Get overlapping appointments.
        var overlappingAppointments = await appointmentRepository.IsOverlappingAppointmentsAsync(
            calendarId,
            period,
            cancellationToken);

        return Result.Success(overlappingAppointments);
    }
}
