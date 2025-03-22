using AgendaManager.Domain.Appointments.ValueObjects;
using AgendaManager.Domain.Common.Interfaces;

namespace AgendaManager.Domain.Appointments.Events;

internal record AppointmentUpdatedDomainEvent(AppointmentId AppointmentId) : IDomainEvent;
