using AgendaManager.Domain.ResourceTypes.ValueObjects;

namespace AgendaManager.Domain.ResourceTypes.Interfaces;

public interface IResourceTypeRepository
{
    Task<ResourceType?> GetByIdAsync(ResourceTypeId resourceTypeId, CancellationToken cancellationToken = default);

    Task<bool> NameExistsAsync(ResourceTypeId resourceTypeId, string name, CancellationToken cancellationToken);

    Task<bool> DescriptionExistsAsync(
        ResourceTypeId resourceTypeId,
        string description,
        CancellationToken cancellationToken);

    Task CreateAsync(ResourceType resourceType, CancellationToken cancellationToken = default);

    void Update(ResourceType resourceType);
}
