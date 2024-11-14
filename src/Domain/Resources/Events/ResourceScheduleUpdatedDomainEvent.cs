using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.Resources.ValueObjects;

namespace AgendaManager.Domain.Resources.Events;

public record ResourceScheduleUpdatedDomainEvent(ResourceId Id, ResourceScheduleId ScheduleId) : IDomainEvent;
