using AgendaManager.Domain.ResourceManagement.Resources.Entities;
using AgendaManager.Domain.ResourceManagement.Resources.Interfaces;
using AgendaManager.Infrastructure.Common.Persistence;

namespace AgendaManager.Infrastructure.ResourceManagement.Resources.Persistence.Repositories;

public class ResourceScheduleRepository(AppDbContext context) : IResourceScheduleRepository
{
    public IQueryable<ResourceSchedule> GetQueryable()
    {
        return context.ResourceSchedules.AsQueryable();
    }
}
