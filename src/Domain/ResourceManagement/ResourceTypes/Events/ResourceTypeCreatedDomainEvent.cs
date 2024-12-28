using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.ValueObjects;

namespace AgendaManager.Domain.ResourceManagement.ResourceTypes.Events;

public record ResourceTypeCreatedDomainEvent(ResourceTypeId ResourceTypeId) : IDomainEvent;
