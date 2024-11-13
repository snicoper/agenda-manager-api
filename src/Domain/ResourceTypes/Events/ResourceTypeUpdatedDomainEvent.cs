using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.ResourceTypes.ValueObjects;

namespace AgendaManager.Domain.ResourceTypes.Events;

internal record ResourceTypeUpdatedDomainEvent(ResourceTypeId Id) : IDomainEvent;
