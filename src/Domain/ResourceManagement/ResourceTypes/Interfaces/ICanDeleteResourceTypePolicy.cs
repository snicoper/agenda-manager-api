namespace AgendaManager.Domain.ResourceManagement.ResourceTypes.Interfaces;

public interface ICanDeleteResourceTypePolicy
{
    Task<bool> CanDeleteAsync(ResourceType resourceType, CancellationToken cancellationToken);
}
