using AgendaManager.Domain.ResourceManagement.ResourceTypes;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.Interfaces;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.ValueObjects;
using AgendaManager.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Infrastructure.ResourceManagement.ResourceTypes.Repositories;

public class ResourceTypeRepository(AppDbContext context) : IResourceTypeRepository
{
    public Task<ResourceType?> GetByIdAsync(
        ResourceTypeId resourceTypeId,
        CancellationToken cancellationToken = default)
    {
        var resourceType = context.ResourceTypes
            .FirstOrDefaultAsync(rt => rt.Id == resourceTypeId, cancellationToken);

        return resourceType;
    }

    public async Task<bool> ExistsByNameAsync(
        ResourceTypeId resourceTypeId,
        string name,
        CancellationToken cancellationToken)
    {
        var exists = await context.ResourceTypes
            .AnyAsync(rt => rt.Name == name && !rt.Id.Equals(resourceTypeId), cancellationToken);

        return exists;
    }

    public async Task<bool> ExistsByDescriptionAsync(
        ResourceTypeId resourceTypeId,
        string description,
        CancellationToken cancellationToken)
    {
        var exists = await context.ResourceTypes
            .AnyAsync(rt => rt.Description == description && rt.Id.Equals(resourceTypeId), cancellationToken);

        return exists;
    }

    public async Task CreateAsync(ResourceType resourceType, CancellationToken cancellationToken = default)
    {
        await context.ResourceTypes.AddAsync(resourceType, cancellationToken);
    }

    public void Update(ResourceType resourceType)
    {
        context.ResourceTypes.Update(resourceType);
    }

    public void Delete(ResourceType resourceType)
    {
        context.ResourceTypes.Remove(resourceType);
    }
}
