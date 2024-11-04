using AgendaManager.Domain.Appointments.Enums;
using AgendaManager.Domain.Appointments.Events;
using AgendaManager.Domain.Appointments.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.ValueObjects;

namespace AgendaManager.Domain.Appointments;

public class Appointment : AggregateRoot
{
    private Appointment(AppointmentId appointmentId, Period period)
    {
        Id = appointmentId;
        Period = period;
    }

    public AppointmentId Id { get; }

    public Period Period { get; private set; }

    public AppointmentStatus Status { get; private set; } = AppointmentStatus.Pending;

    public static Appointment Create(AppointmentId id, Period period)
    {
        Appointment appointment = new(id, period);

        appointment.AddDomainEvent(new AppointmentCreatedDomainEvent(appointment.Id));

        return appointment;
    }
}
