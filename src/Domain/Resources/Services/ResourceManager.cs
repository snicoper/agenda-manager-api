using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects.ColorScheme;
using AgendaManager.Domain.Resources.Errors;
using AgendaManager.Domain.Resources.Interfaces;
using AgendaManager.Domain.Resources.ValueObjects;
using AgendaManager.Domain.ResourceTypes.ValueObjects;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Resources.Services;

public class ResourceManager(IResourceRepository resourceRepository)
{
    public async Task<Result<Resource>> CreateResourceAsync(
        ResourceId resourceId,
        UserId? userId,
        CalendarId calendarId,
        ResourceTypeId typeId,
        string name,
        string description,
        ColorScheme colorScheme,
        bool isActive,
        CancellationToken cancellationToken)
    {
        if (await NameExistsAsync(resourceId, name, cancellationToken))
        {
            return ResourceErrors.NameAlreadyExists;
        }

        if (await DescriptionExistsAsync(resourceId, description, cancellationToken))
        {
            return ResourceErrors.DescriptionAlreadyExists;
        }

        var resource = Resource.Create(
            resourceId,
            userId,
            calendarId,
            typeId,
            name,
            description,
            colorScheme,
            isActive);

        return resource;
    }

    public async Task<Result<Resource>> UpdateResourceAsync(
        ResourceId resourceId,
        string name,
        string description,
        ColorScheme colorScheme,
        CancellationToken cancellationToken)
    {
        var resource = await resourceRepository.GetByIdAsync(resourceId, cancellationToken);

        if (resource is null)
        {
            return ResourceErrors.NotFound;
        }

        if (await NameExistsAsync(resourceId, name, cancellationToken))
        {
            return ResourceErrors.NameAlreadyExists;
        }

        if (await DescriptionExistsAsync(resourceId, description, cancellationToken))
        {
            return ResourceErrors.DescriptionAlreadyExists;
        }

        resource.Update(name, description, colorScheme);

        return resource;
    }

    private async Task<bool> NameExistsAsync(ResourceId resourceId, string name, CancellationToken cancellationToken)
    {
        var exists = await resourceRepository.NameExistsAsync(resourceId, name, cancellationToken);

        return exists;
    }

    private async Task<bool> DescriptionExistsAsync(
        ResourceId resourceId,
        string name,
        CancellationToken cancellationToken)
    {
        var exists = await resourceRepository.DescriptionExistsAsync(resourceId, name, cancellationToken);

        return exists;
    }
}
