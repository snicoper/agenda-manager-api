using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Resources.ValueObjects;

namespace AgendaManager.Domain.Resources.Events;

public class ResourceNameAddedDomainEvent(ResourceId resourceId)
    : DomainEvent
{
    public ResourceId ResourceId { get; } = resourceId;
}
