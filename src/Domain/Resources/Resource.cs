using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Resources.ValueObjects;

namespace AgendaManager.Domain.Resources;

public class Resource : AggregateRoot
{
    private Resource()
    {
    }

    private Resource(ResourceId id, string name)
    {
        Id = id;
        Name = name;
    }

    public ResourceId Id { get; } = null!;

    public string Name { get; private set; } = default!;

    public static Resource Create(ResourceId id, string name)
    {
        return new Resource(id, name);
    }
}
