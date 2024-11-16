using AgendaManager.Domain.Calendars.Constants;
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
                category: CalendarConfigurationKeys.HolidayCreateStrategy.Key,
                key: CalendarConfigurationKeys.HolidayCreateStrategy.RejectIfOverlapping,
                description: CalendarConfigurationKeys.HolidayCreateStrategy.RejectIfOverlappingDescription,
                defaultValue: true),

            CalendarConfigurationOption.Create(
                optionId: CalandarConfigurationOptionId.Create(),
                category: CalendarConfigurationKeys.HolidayCreateStrategy.Key,
                key: CalendarConfigurationKeys.HolidayCreateStrategy.CancelOverlapping,
                description: CalendarConfigurationKeys.HolidayCreateStrategy.CancelOverlappingDescription),

            CalendarConfigurationOption.Create(
                optionId: CalandarConfigurationOptionId.Create(),
                category: CalendarConfigurationKeys.HolidayCreateStrategy.Key,
                key: CalendarConfigurationKeys.HolidayCreateStrategy.AllowOverlapping,
                description: CalendarConfigurationKeys.HolidayCreateStrategy.AllowOverlappingDescription),

            CalendarConfigurationOption.Create(
                optionId: CalandarConfigurationOptionId.Create(),
                category: CalendarConfigurationKeys.AppointmentOverlappingStrategy.Key,
                key: CalendarConfigurationKeys.AppointmentOverlappingStrategy.RejectIfOverlapping,
                description: CalendarConfigurationKeys.AppointmentOverlappingStrategy.RejectIfOverlappingDescription,
                defaultValue: true),

            CalendarConfigurationOption.Create(
                optionId: CalandarConfigurationOptionId.Create(),
                category: CalendarConfigurationKeys.AppointmentOverlappingStrategy.Key,
                key: CalendarConfigurationKeys.AppointmentOverlappingStrategy.AllowOverlapping,
                description: CalendarConfigurationKeys.AppointmentOverlappingStrategy.AllowOverlappingDescription),

            CalendarConfigurationOption.Create(
                optionId: CalandarConfigurationOptionId.Create(),
                category: CalendarConfigurationKeys.AppointmentCreationStrategy.Key,
                key: CalendarConfigurationKeys.AppointmentCreationStrategy.Direct,
                description: CalendarConfigurationKeys.AppointmentCreationStrategy.DirectDescription,
                defaultValue: true),

            CalendarConfigurationOption.Create(
                optionId: CalandarConfigurationOptionId.Create(),
                category: CalendarConfigurationKeys.AppointmentCreationStrategy.Key,
                key: CalendarConfigurationKeys.AppointmentCreationStrategy.RequireConfirmation,
                description: CalendarConfigurationKeys.AppointmentCreationStrategy.RequireConfirmationDescription),

            CalendarConfigurationOption.Create(
                optionId: CalandarConfigurationOptionId.Create(),
                category: CalendarConfigurationKeys.TimeZone.IanaTimeZone,
                key: CalendarConfigurationKeys.TimeZone.Key,
                description: "Time zone",
                defaultValue: true)
        ];

        context.CalendarConfigurationOptions.AddRange(configurations);
        await context.SaveChangesAsync();
    }
}
