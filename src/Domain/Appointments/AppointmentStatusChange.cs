using AgendaManager.Domain.Appointments.Events;
using AgendaManager.Domain.Appointments.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;

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

    public static AppointmentStatusChange Create(AppointmentStatusChangeId appointmentStatusChangeId)
    {
        AppointmentStatusChange appointmentStatusChange = new(appointmentStatusChangeId);

        appointmentStatusChange.AddDomainEvent(
            new AppointmentStatusChangeCreatedDomainEvent(appointmentStatusChange.Id));

        return appointmentStatusChange;
    }
}
