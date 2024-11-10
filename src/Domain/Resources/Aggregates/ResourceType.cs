using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Resources.Events;
using AgendaManager.Domain.Resources.ValueObjects;
using AgendaManager.Domain.Services.Aggregates;

namespace AgendaManager.Domain.Resources.Aggregates;

public class ResourceType : AuditableEntity
{
    private readonly List<Resource> _resources = [];
    private readonly List<Service> _services = [];

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

    public bool RequiredRole { get; private set; }

    public IReadOnlyList<Resource> Resources => _resources.AsReadOnly();

    public IReadOnlyList<Service> Services => _services.AsReadOnly();

    public static ResourceType Create(ResourceTypeId id, string name, string description)
    {
        ResourceType resourceType = new(id, name, description);

        resourceType.AddDomainEvent(new ResourceTypeCreatedDomainEvent(resourceType));

        return resourceType;
    }
}
