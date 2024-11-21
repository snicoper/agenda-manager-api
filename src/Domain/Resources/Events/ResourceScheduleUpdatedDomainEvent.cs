using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.Resources.ValueObjects;

namespace AgendaManager.Domain.Resources.Events;

public record ResourceScheduleUpdatedDomainEvent(ResourceId ResourceId, ResourceScheduleId ScheduleId) : IDomainEvent;
