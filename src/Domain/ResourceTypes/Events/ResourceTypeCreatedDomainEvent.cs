using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.ResourceTypes.ValueObjects;

namespace AgendaManager.Domain.ResourceTypes.Events;

public record ResourceTypeCreatedDomainEvent(ResourceTypeId ResourceTypeId) : IDomainEvent;
