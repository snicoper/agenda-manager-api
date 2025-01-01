using AgendaManager.Domain.Appointments.ValueObjects;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Common.WekDays;
using AgendaManager.Domain.Services.ValueObjects;

namespace AgendaManager.Domain.Appointments.Interfaces;

public interface IAppointmentRepository
{
    Task<Appointment?> GetByIdAsync(AppointmentId appointmentId, CancellationToken cancellationToken = default);

    List<Appointment> GetAllByServiceId(ServiceId serviceId, CancellationToken cancellationToken = default);

    Task<List<Appointment>> GetOverlappingAppointmentsAsync(
        CalendarId calendarId,
        Period period,
        CancellationToken cancellationToken = default);

    Task<List<Appointment>> GetOverlappingAppointmentsByWeekDaysAsync(
        CalendarId calendarId,
        WeekDays addedDays,
        CancellationToken cancellationToken);

    Task<bool> IsOverlappingAppointmentsAsync(
        CalendarId calendarId,
        Period period,
        CancellationToken cancellationToken = default);

    Task<bool> HasAppointmentsInCalendarAsync(CalendarId calendarId, CancellationToken cancellationToken = default);

    Task AddAsync(Result<Appointment> appointment, CancellationToken cancellationToken = default);

    void Update(Appointment appointment);

    void UpdateRange(List<Appointment> appointments);

    void Delete(Appointment appointment);
}
