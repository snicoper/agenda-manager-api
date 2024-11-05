using AgendaManager.Domain.Common.Interfaces;

namespace AgendaManager.Domain.Resources.Events;

public record ResourceTypeCreatedDomainEvent(ResourceType ResourceType) : IDomainEvent;
