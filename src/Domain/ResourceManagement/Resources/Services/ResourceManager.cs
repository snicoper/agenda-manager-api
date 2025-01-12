using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.ResourceManagement.Resources.Errors;
using AgendaManager.Domain.ResourceManagement.Resources.Interfaces;
using AgendaManager.Domain.ResourceManagement.Resources.ValueObjects;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.ValueObjects;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.ResourceManagement.Resources.Services;

public class ResourceManager(IResourceRepository resourceRepository, ICanDeleteResourcePolicy canDeleteResourcePolicy)
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
        if (await ExistsByNameAsync(calendarId, resourceId, name, cancellationToken))
        {
            return ResourceErrors.NameAlreadyExists;
        }

        if (await ExistsByDescriptionAsync(calendarId, resourceId, description, cancellationToken))
        {
            return ResourceErrors.DescriptionAlreadyExists;
        }

        var resource = Resource.Create(
            id: resourceId,
            userId: userId,
            calendarId: calendarId,
            typeId: typeId,
            name: name,
            description: description,
            colorScheme: colorScheme,
            isActive: isActive);

        await resourceRepository.AddAsync(resource, cancellationToken);

        return Result.Success(resource);
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

        if (await ExistsByNameAsync(resource.CalendarId, resourceId, name, cancellationToken))
        {
            return ResourceErrors.NameAlreadyExists;
        }

        if (await ExistsByDescriptionAsync(resource.CalendarId, resourceId, description, cancellationToken))
        {
            return ResourceErrors.DescriptionAlreadyExists;
        }

        resource.Update(name, description, colorScheme);

        resourceRepository.Update(resource);

        return Result.Success(resource);
    }

    public async Task<Result> DeleteResourceAsync(ResourceId resourceId, CancellationToken cancellationToken)
    {
        var resource = await resourceRepository.GetByIdAsync(resourceId, cancellationToken);

        if (resource is null)
        {
            return ResourceErrors.NotFound;
        }

        if (!await canDeleteResourcePolicy.CanDeleteAsync(resource.Id, cancellationToken))
        {
            return ResourceErrors.CannotDelete;
        }

        resourceRepository.Delete(resource);

        return Result.Success();
    }

    private async Task<bool> ExistsByNameAsync(
        CalendarId calendarId,
        ResourceId resourceId,
        string name,
        CancellationToken cancellationToken)
    {
        var exists = await resourceRepository.ExistsByNameAsync(
            calendarId: calendarId,
            resourceId: resourceId,
            name: name,
            cancellationToken: cancellationToken);

        return exists;
    }

    private async Task<bool> ExistsByDescriptionAsync(
        CalendarId calendarId,
        ResourceId resourceId,
        string name,
        CancellationToken cancellationToken)
    {
        var exists = await resourceRepository.ExistsByDescriptionAsync(
            calendarId: calendarId,
            resourceId: resourceId,
            name: name,
            cancellationToken: cancellationToken);

        return exists;
    }
}
