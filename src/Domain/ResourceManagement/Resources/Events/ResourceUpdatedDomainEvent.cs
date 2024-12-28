using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.ResourceManagement.Resources.ValueObjects;

namespace AgendaManager.Domain.ResourceManagement.Resources.Events;

internal record ResourceUpdatedDomainEvent(ResourceId ResourceId) : IDomainEvent;
