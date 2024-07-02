using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Resources.ValueObjects;

namespace AgendaManager.Domain.Resources;

public class Resource(ResourceId id) : AuditableEntity
{
    public ResourceId Id { get; private set; } = id;
}
