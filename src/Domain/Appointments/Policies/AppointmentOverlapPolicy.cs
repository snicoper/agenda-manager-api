using AgendaManager.Domain.Appointments.Errors;
using AgendaManager.Domain.Appointments.Interfaces;
using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.Enums;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;

namespace AgendaManager.Domain.Appointments.Policies;

public class AppointmentOverlapPolicy(IAppointmentRepository appointmentRepository)
    : IAppointmentOverlapPolicy
{
    public async Task<Result> IsOverlappingAsync(
        Calendar calendar,
        Period period,
        CancellationToken cancellationToken = default)
    {
        // 1. Allow overlapping.
        if (calendar.Settings.OverlapBehavior is AppointmentOverlappingStrategy.Allow)
        {
            return Result.Success();
        }

        // 3. Get overlapping appointments.
        var overlappingAppointments = await appointmentRepository.IsOverlappingAppointmentsAsync(
            calendar.Id,
            period,
            cancellationToken);

        return overlappingAppointments
            ? AppointmentErrors.AppointmentsOverlapping
            : Result.Success();
    }
}
