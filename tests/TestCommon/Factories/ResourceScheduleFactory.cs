using AgendaManager.Domain.Resources.Entities;
using AgendaManager.Domain.Resources.ValueObjects;

namespace AgendaManager.TestCommon.Factories;

public static class ResourceScheduleFactory
{
    public static ResourceSchedule CreateResourceSchedule(ResourceScheduleId? resourceScheduleId = null)
    {
        var resourceSchedule = ResourceSchedule.Create(
            resourceScheduleId: resourceScheduleId ?? ResourceScheduleId.Create());

        return resourceSchedule;
    }
}
