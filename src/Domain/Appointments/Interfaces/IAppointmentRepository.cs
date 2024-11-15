using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.ValueObjects.Period;
using AgendaManager.Domain.Services.ValueObjects;

namespace AgendaManager.Domain.Appointments.Interfaces;

public interface IAppointmentRepository
{
    Task<List<Appointment>> GetOverlappingAppointmentsAsync(
        CalendarId calendarId,
        Period period,
        CancellationToken cancellationToken = default);

    List<Appointment> GetAllByServiceId(ServiceId serviceId, CancellationToken cancellationToken = default);
}
