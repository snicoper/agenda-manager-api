using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Common.WeekDays;
using AgendaManager.Domain.ResourceManagement.Resources.Entities;
using AgendaManager.Domain.ResourceManagement.Resources.Enums;
using AgendaManager.Domain.ResourceManagement.Resources.ValueObjects;

namespace AgendaManager.Infrastructure.Common.Persistence.Seeds.Features;

public static class ResourceScheduleSeed
{
    public static async Task InitializeAsync(AppDbContext context)
    {
        if (context.ResourceSchedules.Any())
        {
            return;
        }

        var resource = context.Resources.FirstOrDefault();
        var calendar = context.Calendars.FirstOrDefault();

        if (resource is null || calendar is null)
        {
            throw new Exception("There is a lack of resources to be able to create the seed.");
        }

        var startMorning = new DateTimeOffset(2020, 1, 1, 8, 0, 0, TimeSpan.Zero);
        var endMorning = new DateTimeOffset(2040, 12, 31, 14, 0, 0, TimeSpan.Zero);
        var startAfternoon = new DateTimeOffset(2020, 1, 1, 16, 0, 0, TimeSpan.Zero);
        var endAfternoon = new DateTimeOffset(2040, 12, 31, 20, 0, 0, TimeSpan.Zero);

        var periodMorning = Period.From(startMorning, endMorning);
        var periodAfternoon = Period.From(startAfternoon, endAfternoon);

        var scheduleMorning = ResourceSchedule.Create(
            ResourceScheduleId.Create(),
            resource.Id,
            calendar.Id,
            periodMorning,
            ResourceScheduleType.Available,
            WeekDays.WeekendDays,
            "Morning",
            "Period for morning from");

        var scheduleAfternoon = ResourceSchedule.Create(
            ResourceScheduleId.Create(),
            resource.Id,
            calendar.Id,
            periodAfternoon,
            ResourceScheduleType.Available,
            WeekDays.WeekendDays,
            "Afternoon",
            "Period for afternoon from");

        resource.AddSchedule(scheduleMorning);
        resource.AddSchedule(scheduleAfternoon);

        context.ResourceSchedules.Add(scheduleMorning);
        context.ResourceSchedules.Add(scheduleAfternoon);
        await context.SaveChangesAsync();
    }
}
