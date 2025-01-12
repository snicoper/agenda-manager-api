using AgendaManager.Domain.ResourceManagement.Resources.ValueObjects;

namespace AgendaManager.Domain.ResourceManagement.Resources.Interfaces;

public interface ICanDeleteResourcePolicy
{
    Task<bool> CanDeleteAsync(ResourceId resourceId, CancellationToken cancellationToken);
}
