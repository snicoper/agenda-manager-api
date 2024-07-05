using AgendaManager.Domain.Common.Interfaces;
using AgendaManager.Domain.Resources.ValueObjects;

namespace AgendaManager.Domain.Resources.Events;

public class ResourceNameAddedDomainEvent(ResourceId resourceId)
    : IDomainEvent
{
    public ResourceId ResourceId { get; } = resourceId;
}
