using AgendaManager.Domain.ResourceManagement.ResourceTypes.ValueObjects;

namespace AgendaManager.Domain.ResourceManagement.ResourceTypes.Interfaces;

public interface IResourceTypeRepository
{
    IQueryable<ResourceType> GetQueryable();

    Task<ResourceType?> GetByIdAsync(ResourceTypeId resourceTypeId, CancellationToken cancellationToken = default);

    Task<bool> ExistsByNameAsync(ResourceTypeId resourceTypeId, string name, CancellationToken cancellationToken);

    Task CreateAsync(ResourceType resourceType, CancellationToken cancellationToken = default);

    void Update(ResourceType resourceType);

    void Delete(ResourceType resourceType);
}
