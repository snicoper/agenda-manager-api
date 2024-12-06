using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Common.WekDays;
using AgendaManager.Domain.Resources.Entities;
using AgendaManager.Domain.Resources.Enums;
using AgendaManager.Domain.Resources.ValueObjects;

namespace AgendaManager.TestCommon.Factories;

public static class ResourceScheduleFactory
{
    public static ResourceSchedule CreateResourceSchedule(
        ResourceScheduleId? resourceScheduleId = null,
        ResourceId? resourceId = null,
        CalendarId? calendarId = null,
        Period? period = null,
        ResourceScheduleType? type = null,
        WeekDays? availableDays = null,
        string? name = null,
        string? description = null,
        bool? isActive = null)
    {
        var resourceSchedule = ResourceSchedule.Create(
            resourceScheduleId: resourceScheduleId ?? ResourceScheduleId.Create(),
            resourceId: resourceId ?? ResourceId.Create(),
            calendarId: calendarId ?? CalendarId.Create(),
            period: period ?? Period.From(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(1)),
            type: type ?? ResourceScheduleType.Available,
            availableDays: availableDays ?? WeekDays.WorkDays,
            name: name ?? "Resource Schedule test",
            description: description ?? "Resource Schedule test description",
            isActive: isActive ?? true);

        return resourceSchedule;
    }
}
