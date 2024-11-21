using AgendaManager.Domain.Appointments.ValueObjects;
using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.Common.ValueObjects.Period;
using AgendaManager.Domain.Resources;

namespace AgendaManager.Domain.Appointments.Events;

internal record AppointmentUpdatedDomainEvent(AppointmentId AppointmentId, Period Period, List<Resource> Resources)
    : IDomainEvent;
