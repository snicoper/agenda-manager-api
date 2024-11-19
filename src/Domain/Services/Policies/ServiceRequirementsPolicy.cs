using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Resources;
using AgendaManager.Domain.Services.Errors;
using AgendaManager.Domain.Services.Interfaces;
using AgendaManager.Domain.Services.ValueObjects;

namespace AgendaManager.Domain.Services.Policies;

public class ServiceRequirementsPolicy(IServiceRepository serviceRepository) : IServiceRequirementsPolicy
{
    public async Task<Result> IsSatisfiedByAsync(
        ServiceId serviceId,
        List<Resource> resources,
        CancellationToken cancellationToken = default)
    {
        // 1. Fast fail if no resources provided
        if (resources.Count == 0)
        {
            return ServiceErrors.ResourceRequirementsMismatch;
        }

        // 2. Get service with resource types.
        var service = await serviceRepository.GetByIdWithResourceTypesAsync(serviceId, cancellationToken);

        if (service is null)
        {
            return ServiceErrors.ServiceNotFound;
        }

        // 3. Group resources by type and count them.
        var resourcesByType = resources
            .GroupBy(r => r.TypeId)
            .ToDictionary(g => g.Key, g => g.Count());

        // 4. Group service required resource types and count them.
        var requiredResourcesByType = service.ResourceTypes
            .GroupBy(rt => rt.Id)
            .ToDictionary(g => g.Key, g => g.Count());

        // 5. Check if all required resource types are present with correct quantities.
        foreach (var (resourceTypeId, requiredCount) in requiredResourcesByType)
        {
            if (!resourcesByType.TryGetValue(resourceTypeId, out var providedCount)
                || providedCount != requiredCount)
            {
                return ServiceErrors.ResourceRequirementsMismatch;
            }
        }

        return Result.Success();
    }
}
