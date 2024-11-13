using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Resources;
using AgendaManager.Domain.ResourceTypes.Events;
using AgendaManager.Domain.ResourceTypes.ValueObjects;
using AgendaManager.Domain.Services;
using AgendaManager.Domain.Users.Entities;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.ResourceTypes;

public sealed class ResourceType : AggregateRoot
{
    private readonly List<Resource> _resources = [];
    private readonly List<Service> _services = [];

    private ResourceType()
    {
    }

    private ResourceType(ResourceTypeId id, string name, string description, RoleId? roleId)
    {
        Id = id;
        RoleId = roleId;
        Name = name;
        Description = description;
    }

    public ResourceTypeId Id { get; } = null!;

    public RoleId? RoleId { get; private set; }

    public Role Role { get; private set; } = null!;

    public string Name { get; private set; } = default!;

    public string Description { get; private set; } = default!;

    public IReadOnlyList<Resource> Resources => _resources.AsReadOnly();

    public IReadOnlyList<Service> Services => _services.AsReadOnly();

    public static ResourceType Create(ResourceTypeId id, string name, string description, RoleId? roleId = null)
    {
        ResourceType resourceType = new(id, name, description, roleId);

        resourceType.AddDomainEvent(new ResourceTypeCreatedDomainEvent(resourceType));

        return resourceType;
    }
}
