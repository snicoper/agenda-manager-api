using AgendaManager.Domain.Authorization.Errors;
using AgendaManager.Domain.Authorization.Interfaces;
using AgendaManager.Domain.Authorization.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.ResourceTypes.Errors;
using AgendaManager.Domain.ResourceTypes.Interfaces;
using AgendaManager.Domain.ResourceTypes.ValueObjects;

namespace AgendaManager.Domain.ResourceTypes.Services;

public sealed class ResourceTypeManager(
    IResourceTypeRepository resourceTypeRepository,
    IRoleRepository roleRepository)
{
    public async Task<Result<ResourceType>> CreateResourceTypeAsync(
        ResourceTypeId resourceTypeId,
        string name,
        string description,
        CancellationToken cancellationToken,
        RoleId? roleId = null)
    {
        if (await NameExistsAsync(resourceTypeId, name, cancellationToken))
        {
            return ResourceTypeErrors.NameAlreadyExists;
        }

        if (await DescriptionExistsAsync(resourceTypeId, description, cancellationToken))
        {
            return ResourceTypeErrors.DescriptionExists;
        }

        if (roleId is not null && !await roleRepository.ExistsByIdAsync(roleId, cancellationToken))
        {
            return RoleErrors.RoleNotFound.ToResult<ResourceType>();
        }

        var resourceType = ResourceType.Create(
            resourceTypeId,
            name,
            description,
            roleId);

        await resourceTypeRepository.CreateAsync(resourceType, cancellationToken);

        return Result.Create(resourceType);
    }

    public async Task<Result<ResourceType>> UpdateResourceTypeAsync(
        ResourceTypeId resourceTypeId,
        string name,
        string description,
        CancellationToken cancellationToken)
    {
        if (await NameExistsAsync(resourceTypeId, name, cancellationToken))
        {
            return ResourceTypeErrors.NameAlreadyExists;
        }

        if (await DescriptionExistsAsync(resourceTypeId, description, cancellationToken))
        {
            return ResourceTypeErrors.DescriptionExists;
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

    private async Task<bool> NameExistsAsync(
        ResourceTypeId resourceTypeId,
        string name,
        CancellationToken cancellationToken)
    {
        var exists = await resourceTypeRepository.NameExistsAsync(resourceTypeId, name, cancellationToken);

        return exists;
    }

    private async Task<bool> DescriptionExistsAsync(
        ResourceTypeId resourceTypeId,
        string description,
        CancellationToken cancellationToken)
    {
        var exists = await resourceTypeRepository.DescriptionExistsAsync(
            resourceTypeId,
            description,
            cancellationToken);

        return exists;
    }
}
