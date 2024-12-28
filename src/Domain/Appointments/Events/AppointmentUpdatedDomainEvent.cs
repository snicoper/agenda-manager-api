using AgendaManager.Domain.Appointments.ValueObjects;
using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.ResourceManagement.Resources;

namespace AgendaManager.Domain.Appointments.Events;

internal record AppointmentUpdatedDomainEvent(AppointmentId AppointmentId, Period Period, List<Resource> Resources)
    : IDomainEvent;
