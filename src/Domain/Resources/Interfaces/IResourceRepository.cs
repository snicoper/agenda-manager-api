using AgendaManager.Domain.Resources.ValueObjects;

namespace AgendaManager.Domain.Resources.Interfaces;

public interface IResourceRepository
{
    Task<Resource?> GetByIdAsync(ResourceId resourceId, CancellationToken cancellationToken = default);

    Task<bool> NameExistsAsync(ResourceId resourceId, string name, CancellationToken cancellationToken = default);

    Task<bool> DescriptionExistsAsync(
        ResourceId resourceId,
        string name,
        CancellationToken cancellationToken = default);
}
