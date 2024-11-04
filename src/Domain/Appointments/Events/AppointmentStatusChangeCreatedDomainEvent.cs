using AgendaManager.Domain.Appointments.ValueObjects;
using AgendaManager.Domain.Common.Interfaces;

namespace AgendaManager.Domain.Appointments.Events;

public record AppointmentStatusChangeCreatedDomainEvent(AppointmentStatusChangeId Id) : IDomainEvent;
