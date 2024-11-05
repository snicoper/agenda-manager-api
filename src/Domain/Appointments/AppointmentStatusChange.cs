using AgendaManager.Domain.Appointments.Enums;
using AgendaManager.Domain.Appointments.Events;
using AgendaManager.Domain.Appointments.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.ValueObjects;

namespace AgendaManager.Domain.Appointments;

public class AppointmentStatusChange : AuditableEntity
{
    private AppointmentStatusChange()
    {
    }

    private AppointmentStatusChange(AppointmentStatusChangeId appointmentStatusChangeId)
    {
        Id = appointmentStatusChangeId;
    }

    public AppointmentStatusChangeId Id { get; } = null!;

    public AppointmentId AppointmentId { get; private set; } = null!;

    public Appointment Appointment { get; private set; } = null!;

    public Period Period { get; private set; } = null!;

    public AppointmentStatus Status { get; private set; } = AppointmentStatus.Pending;

    public bool IsCurrentStatus { get; private set; } = true;

    public TimeSpan Duration { get; private set; } = TimeSpan.Zero;

    public string? Description { get; private set; }

    public static AppointmentStatusChange Create(AppointmentStatusChangeId appointmentStatusChangeId)
    {
        AppointmentStatusChange appointmentStatusChange = new(appointmentStatusChangeId);

        appointmentStatusChange.AddDomainEvent(
            new AppointmentStatusChangeCreatedDomainEvent(appointmentStatusChange.Id));

        return appointmentStatusChange;
    }
}
