using AgendaManager.Domain.ResourceManagement.Resources.Entities;

namespace AgendaManager.Domain.ResourceManagement.Resources.Interfaces;

public interface IResourceScheduleRepository
{
    IQueryable<ResourceSchedule> GetQueryable();
}
