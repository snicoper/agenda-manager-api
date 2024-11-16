using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.ValueObjects;

namespace AgendaManager.Infrastructure.Common.Persistence.Seeds.Modules;

public static class ConfigurationSeed
{
    public static async Task InitializeAsync(AppDbContext context)
    {
        if (context.CalendarConfigurationOptions.Any())
        {
            return;
        }

        List<CalendarConfigurationOption> configurations =
        [
            CalendarConfigurationOption.Create(
                optionId: CalandarConfigurationOptionId.Create(),
                category: "HolidayCreateStrategy",
                key: "RejectIfOverlapping",
                description: "Reject if overlapping",
                defaultValue: true),
            CalendarConfigurationOption.Create(
                optionId: CalandarConfigurationOptionId.Create(),
                category: "HolidayCreateStrategy",
                key: "CancelOverlapping",
                description: "Cancel overlapping"),
            CalendarConfigurationOption.Create(
                optionId: CalandarConfigurationOptionId.Create(),
                category: "HolidayCreateStrategy",
                key: "AllowOverlapping",
                description: "Allow overlapping"),
            CalendarConfigurationOption.Create(
                optionId: CalandarConfigurationOptionId.Create(),
                category: "AppointmentOverlappingStrategy",
                key: "RejectIfOverlapping",
                description: "Reject if overlapping",
                defaultValue: true),
            CalendarConfigurationOption.Create(
                optionId: CalandarConfigurationOptionId.Create(),
                category: "AppointmentOverlappingStrategy",
                key: "AllowOverlapping",
                description: "Allow overlapping"),
            CalendarConfigurationOption.Create(
                optionId: CalandarConfigurationOptionId.Create(),
                category: "AppointmentCreationStrategy",
                key: "Direct",
                description: "Direct",
                defaultValue: true),
            CalendarConfigurationOption.Create(
                optionId: CalandarConfigurationOptionId.Create(),
                category: "AppointmentCreationStrategy",
                key: "RequireConfirmation",
                description: "RequireConfiguration"),
            CalendarConfigurationOption.Create(
                optionId: CalandarConfigurationOptionId.Create(),
                category: "IanaTimeZone",
                key: "Value",
                description: "Time zone",
                defaultValue: true)
        ];

        context.CalendarConfigurationOptions.AddRange(configurations);
        await context.SaveChangesAsync();
    }
}
