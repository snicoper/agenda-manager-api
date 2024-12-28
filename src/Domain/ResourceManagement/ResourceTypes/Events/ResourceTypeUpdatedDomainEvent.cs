using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.ValueObjects;

namespace AgendaManager.Domain.ResourceManagement.ResourceTypes.Events;

internal record ResourceTypeUpdatedDomainEvent(ResourceTypeId ResourceTypeId) : IDomainEvent;
