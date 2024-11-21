using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.Resources.ValueObjects;

namespace AgendaManager.Domain.Resources.Events;

internal record ResourceUpdatedDomainEvent(ResourceId ResourceId) : IDomainEvent;
