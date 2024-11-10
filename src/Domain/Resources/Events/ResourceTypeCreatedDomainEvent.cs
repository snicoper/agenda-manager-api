using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.Resources.Aggregates;

namespace AgendaManager.Domain.Resources.Events;

public record ResourceTypeCreatedDomainEvent(ResourceType ResourceType) : IDomainEvent;
