using AgendaManager.Domain.Appointments;
using AgendaManager.Domain.Appointments.Interfaces;
using AgendaManager.Domain.Appointments.ValueObjects;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects.Period;
using AgendaManager.Domain.Services.ValueObjects;
using AgendaManager.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Infrastructure.Appointments.Persistence.Repositories;

public class AppointmentRepository(AppDbContext context) : IAppointmentRepository
{
    public async Task<Appointment?> GetByIdAsync(
        AppointmentId appointmentId,
        CancellationToken cancellationToken = default)
    {
        var appointment = await context.Appointments.FirstOrDefaultAsync(a => a.Id == appointmentId, cancellationToken);

        return appointment;
    }

    public Task<List<Appointment>> GetOverlappingAppointmentsAsync(
        CalendarId calendarId,
        Period period,
        CancellationToken cancellationToken = default)
    {
        var overlappingAppointments = context.Appointments
            .Where(
                a => a.CalendarId == calendarId
                     && a.Period.Start < period.End
                     && a.Period.End > period.Start)
            .ToListAsync(cancellationToken);

        return overlappingAppointments;
    }

    public async Task<bool> IsOverlappingAppointmentsAsync(
        CalendarId calendarId,
        Period period,
        CancellationToken cancellationToken = default)
    {
        var overlappingAppointments = await context.Appointments
            .AnyAsync(
                a => a.CalendarId == calendarId
                     && a.Period.Start < period.End
                     && a.Period.End > period.Start,
                cancellationToken);

        return overlappingAppointments;
    }

    public async Task<bool> HasAppointmentsInCalendarAsync(
        CalendarId calendarId,
        CancellationToken cancellationToken = default)
    {
        var hasAppointments = await context.Appointments
            .AnyAsync(a => a.CalendarId == calendarId, cancellationToken);

        return hasAppointments;
    }

    public List<Appointment> GetAllByServiceId(
        ServiceId serviceId,
        CancellationToken cancellationToken = default)
    {
        var appointments = context.Appointments.Where(a => a.ServiceId == serviceId);

        return appointments.ToList();
    }

    public async Task AddAsync(Result<Appointment> appointment, CancellationToken cancellationToken = default)
    {
        await context.Appointments.AddAsync(appointment.Value!, cancellationToken);
    }

    public void Update(Appointment appointment)
    {
        context.Appointments.Update(appointment);
    }

    public void Delete(Appointment appointment)
    {
        context.Appointments.Remove(appointment);
    }
}
