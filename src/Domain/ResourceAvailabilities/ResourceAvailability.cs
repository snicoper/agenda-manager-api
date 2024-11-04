using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.ResourceAvailabilities.Events;
using AgendaManager.Domain.ResourceAvailabilities.ValueObjects;

namespace AgendaManager.Domain.ResourceAvailabilities;

public class ResourceAvailability : AggregateRoot
{
    private ResourceAvailability()
    {
    }

    private ResourceAvailability(ResourceAvailabilityId id)
    {
        Id = id;
    }

    public ResourceAvailabilityId Id { get; } = null!;

    public static ResourceAvailability Create(ResourceAvailabilityId id)
    {
        ResourceAvailability resourceAvailability = new(id);

        resourceAvailability.AddDomainEvent(new ResourceAvailabilityCreatedDomainEvent(resourceAvailability.Id));

        return resourceAvailability;
    }
}
