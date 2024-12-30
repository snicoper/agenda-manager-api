using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.Errors;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.Interfaces;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.ValueObjects;
using AgendaManager.Domain.ResourceManagement.Shared.Enums;

namespace AgendaManager.Domain.ResourceManagement.ResourceTypes.Services;

public sealed class ResourceTypeManager(
    IResourceTypeRepository resourceTypeRepository,
    ICanDeleteResourceTypePolicy canDeleteResourceTypePolicy)
{
    public async Task<Result<ResourceType>> CreateResourceTypeAsync(
        ResourceTypeId resourceTypeId,
        string name,
        string description,
        ResourceCategory category,
        CancellationToken cancellationToken)
    {
        if (await ExistsByNameAsync(resourceTypeId, name, cancellationToken))
        {
            return ResourceTypeErrors.NameAlreadyExists;
        }

        var resourceType = ResourceType.Create(
            id: resourceTypeId,
            name: name,
            description: description,
            category: category);

        await resourceTypeRepository.CreateAsync(resourceType, cancellationToken);

        return Result.Create(resourceType);
    }

    public async Task<Result<ResourceType>> UpdateResourceTypeAsync(
        ResourceTypeId resourceTypeId,
        string name,
        string description,
        CancellationToken cancellationToken)
    {
        if (await ExistsByNameAsync(resourceTypeId, name, cancellationToken))
        {
            return ResourceTypeErrors.NameAlreadyExists;
        }

        var resourceType = await resourceTypeRepository.GetByIdAsync(resourceTypeId, cancellationToken);

        if (resourceType is null)
        {
            return ResourceTypeErrors.ResourceTypeNotFound;
        }

        resourceType.Update(name, description);

        resourceTypeRepository.Update(resourceType);

        return Result.Success(resourceType);
    }

    public async Task<Result> DeleteResourceTypeAsync(
        ResourceTypeId resourceTypeId,
        CancellationToken cancellationToken)
    {
        var resourceType = await resourceTypeRepository.GetByIdAsync(resourceTypeId, cancellationToken);

        if (resourceType is null)
        {
            return ResourceTypeErrors.ResourceTypeNotFound;
        }

        var canDelete = await canDeleteResourceTypePolicy.CanDeleteAsync(resourceType, cancellationToken);

        if (!canDelete)
        {
            return ResourceTypeErrors.CannotDeleteResourceType;
        }

        resourceTypeRepository.Delete(resourceType);

        return Result.Success();
    }

    private async Task<bool> ExistsByNameAsync(
        ResourceTypeId resourceTypeId,
        string name,
        CancellationToken cancellationToken)
    {
        var exists = await resourceTypeRepository.ExistsByNameAsync(resourceTypeId, name, cancellationToken);

        return exists;
    }
}
