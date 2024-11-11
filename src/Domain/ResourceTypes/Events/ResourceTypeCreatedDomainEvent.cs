using AgendaManager.Domain.Common.Interfaces;

namespace AgendaManager.Domain.ResourceTypes.Events;

public record ResourceTypeCreatedDomainEvent(ResourceType ResourceType) : IDomainEvent;
