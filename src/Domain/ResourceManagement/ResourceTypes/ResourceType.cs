using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.ResourceManagement.Resources;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.Events;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.Exceptions;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.ValueObjects;
using AgendaManager.Domain.ResourceManagement.Shared.Enums;
using AgendaManager.Domain.Services;

namespace AgendaManager.Domain.ResourceManagement.ResourceTypes;

public sealed class ResourceType : AggregateRoot
{
    private readonly List<Resource> _resources = [];
    private readonly List<Service> _services = [];

    private ResourceType()
    {
    }

    private ResourceType(ResourceTypeId id, string name, string description, ResourceCategory category)
    {
        GuardAgainstInvalidName(name);
        GuardAgainstInvalidDescription(description);

        Id = id;
        Name = name;
        Description = description;
        Category = category;
    }

    public ResourceTypeId Id { get; } = null!;

    public string Name { get; private set; } = null!;

    public string Description { get; private set; } = null!;

    public ResourceCategory Category { get; private set; }

    public IReadOnlyList<Resource> Resources => _resources.AsReadOnly();

    public IReadOnlyList<Service> Services => _services.AsReadOnly();

    internal static ResourceType Create(ResourceTypeId id, string name, string description, ResourceCategory category)
    {
        ResourceType resourceType = new(id, name, description, category);

        resourceType.AddDomainEvent(new ResourceTypeCreatedDomainEvent(resourceType.Id));

        return resourceType;
    }

    internal void Update(string name, string description)
    {
        GuardAgainstInvalidName(name);
        GuardAgainstInvalidDescription(description);

        if (Name == name && Description == description)
        {
            return;
        }

        Name = name;
        Description = description;

        AddDomainEvent(new ResourceTypeUpdatedDomainEvent(Id));
    }

    private static void GuardAgainstInvalidName(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        if (string.IsNullOrWhiteSpace(name) || name.Length > 50)
        {
            throw new ResourceTypeDomainException("Name is empty or exceeds length of 50 characters.");
        }
    }

    private static void GuardAgainstInvalidDescription(string description)
    {
        ArgumentNullException.ThrowIfNull(description);

        if (string.IsNullOrWhiteSpace(description) || description.Length > 500)
        {
            throw new ResourceTypeDomainException("Description is invalid or exceeds length of 50 characters.");
        }
    }
}
