using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Resources;
using AgendaManager.Domain.Resources.ValueObjects;
using AgendaManager.Domain.ResourceSchedules.Enums;
using AgendaManager.Domain.ResourceSchedules.Events;
using AgendaManager.Domain.ResourceSchedules.ValueObjects;

namespace AgendaManager.Domain.ResourceSchedules;

public class ResourceSchedule : AggregateRoot
{
    private ResourceSchedule()
    {
    }

    private ResourceSchedule(ResourceScheduleId resourceScheduleId)
    {
        Id = resourceScheduleId;
    }

    public ResourceScheduleId Id { get; } = null!;

    public ResourceId ResourceId { get; private set; } = null!;

    public Resource Resource { get; private set; } = null!;

    public CalendarId CalendarId { get; private set; } = null!;

    public Calendar Calendar { get; private set; } = null!;

    public Period Period { get; private set; } = null!;

    public ResourceScheduleType Type { get; private set; }

    public List<DayOfWeek> AvailableDays { get; private set; } = null!;

    public string Name { get; private set; } = null!;

    public string Description { get; private set; } = default!;

    public static ResourceSchedule Create(ResourceScheduleId resourceScheduleId)
    {
        ResourceSchedule resourceSchedule = new(resourceScheduleId);

        resourceSchedule.AddDomainEvent(new ResourceScheduleCreatedDomainEvent(resourceSchedule.Id));

        return resourceSchedule;
    }
}
