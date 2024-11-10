using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.ValueObjects.Period;
using AgendaManager.Domain.Common.WekDays;
using AgendaManager.Domain.Resources.Enums;
using AgendaManager.Domain.Resources.Events;
using AgendaManager.Domain.Resources.ValueObjects;

namespace AgendaManager.Domain.Resources.Entities;

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

    public ResourceScheduleType Type { get; private set; } = ResourceScheduleType.Available;

    public WeekDays AvailableDays { get; private set; } = WeekDays.None;

    public string Name { get; private set; } = null!;

    public string Description { get; private set; } = default!;

    public static ResourceSchedule Create(ResourceScheduleId resourceScheduleId)
    {
        ResourceSchedule resourceSchedule = new(resourceScheduleId);

        resourceSchedule.AddDomainEvent(new ResourceScheduleCreatedDomainEvent(resourceSchedule.Id));

        return resourceSchedule;
    }
}
