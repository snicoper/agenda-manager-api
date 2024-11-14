using AgendaManager.Domain.Resources;
using AgendaManager.Domain.Resources.Interfaces;
using AgendaManager.Domain.Resources.ValueObjects;
using AgendaManager.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgendaManager.Infrastructure.Resources.Repositories;

public class ResourceRepository(AppDbContext context) : IResourceRepository
{
    public async Task<Resource?> GetByIdAsync(ResourceId resourceId, CancellationToken cancellationToken = default)
    {
        var resource = await context.Resources.FirstOrDefaultAsync(r => r.Id == resourceId, cancellationToken);

        return resource;
    }

    public async Task<bool> NameExistsAsync(
        ResourceId resourceId,
        string name,
        CancellationToken cancellationToken = default)
    {
        var exists = await context.Resources.AnyAsync(r => r.Id != resourceId && r.Name == name, cancellationToken);

        return exists;
    }

    public async Task<bool> DescriptionExistsAsync(
        ResourceId resourceId,
        string name,
        CancellationToken cancellationToken = default)
    {
        var exists = await context.Resources.AnyAsync(
            r => r.Id != resourceId && r.Description == name,
            cancellationToken);

        return exists;
    }
}
