using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Resources.Events;
using AgendaManager.Domain.Resources.ValueObjects;

namespace AgendaManager.Domain.Resources;

public class ResourceType : AuditableEntity
{
    private ResourceType()
    {
    }

    private ResourceType(ResourceTypeId id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public ResourceTypeId Id { get; private set; } = null!;

    public string Name { get; private set; } = default!;

    public string Description { get; private set; } = default!;

    public static ResourceType Create(ResourceTypeId id, string name, string description)
    {
        ResourceType resourceType = new(id, name, description);

        resourceType.AddDomainEvent(new ResourceTypeCreatedDomainEvent(resourceType));

        return resourceType;
    }
}
