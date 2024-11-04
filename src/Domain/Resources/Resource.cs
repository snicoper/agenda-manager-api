using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Resources.Events;
using AgendaManager.Domain.Resources.ValueObjects;

namespace AgendaManager.Domain.Resources;

public sealed class Resource : AggregateRoot
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
        Resource resource = new(id, name);

        resource.AddDomainEvent(new ResourceCreatedDomainEvent(resource.Id));

        return resource;
    }
}
