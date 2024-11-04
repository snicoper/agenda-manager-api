using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.ResourceSchedules.ValueObjects;

namespace AgendaManager.Domain.ResourceSchedules.Events;

public record ResourceScheduleCreatedDomainEvent(ResourceScheduleId ResourceScheduleId) : IDomainEvent;
