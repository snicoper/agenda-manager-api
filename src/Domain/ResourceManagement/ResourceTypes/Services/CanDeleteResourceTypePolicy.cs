using AgendaManager.Domain.ResourceManagement.Resources.Interfaces;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.Interfaces;
using AgendaManager.Domain.Services.Interfaces;

namespace AgendaManager.Domain.ResourceManagement.ResourceTypes.Services;

public class CanDeleteResourceTypePolicy(IResourceRepository resourceRepository, IServiceRepository serviceRepository)
    : ICanDeleteResourceTypePolicy
{
    public async Task<bool> CanDeleteAsync(ResourceType resourceType, CancellationToken cancellationToken)
    {
        var anyResources = await resourceRepository.AnyByTypeIdAsync(resourceType.Id, cancellationToken);

        if (anyResources)
        {
            return false;
        }

        var anyServices = await serviceRepository.AnyByResourceTypeIdAsync(resourceType.Id, cancellationToken);

        return !anyServices;
    }
}
