using AgendaManager.Domain.Appointments;
using AgendaManager.Domain.Appointments.Interfaces;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.ValueObjects.Period;

namespace AgendaManager.Infrastructure.Appointments.Persistence.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    public Task<List<Appointment>> GetOverlappingAppointmentsAsync(
        CalendarId calendarId,
        Period period,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult<List<Appointment>>([]);
    }
}
