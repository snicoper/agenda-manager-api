using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.Resources.ValueObjects;

namespace AgendaManager.Domain.Resources.Events;

public record ResourceScheduleCreatedDomainEvent(ResourceScheduleId ResourceScheduleId) : IDomainEvent;
