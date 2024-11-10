using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.Resources.Entities;

namespace AgendaManager.Domain.Resources.Events;

public record ResourceTypeCreatedDomainEvent(ResourceType ResourceType) : IDomainEvent;
