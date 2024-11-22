using AgendaManager.Domain.Appointments.Interfaces;
using AgendaManager.Domain.Calendars.ValueObjects;

namespace AgendaManager.Domain.Appointments.Policies;

public class HasAppointmentsInCalendarPolicy(IAppointmentRepository appointmentRepository)
    : IHasAppointmentsInCalendarPolicy
{
    public async Task<bool> IsSatisfiedByAsync(CalendarId calendarId, CancellationToken cancellationToken)
    {
        var hasAppointments = await appointmentRepository.HasAppointmentsInCalendarAsync(calendarId, cancellationToken);

        return hasAppointments;
    }
}
