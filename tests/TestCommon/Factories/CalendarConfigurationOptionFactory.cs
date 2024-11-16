using AgendaManager.Domain.Calendars.Constants;
using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Infrastructure.Common.Persistence.Seeds.Modules;

namespace AgendaManager.TestCommon.Factories;

public static class CalendarConfigurationOptionFactory
{
    /// <summary>
    /// Replica real de datos en la base de datos.
    /// Tener los datos sincronizados con: <see cref="ConfigurationSeed" />.
    /// </summary>
    public static List<CalendarConfigurationOption> GetAll()
    {
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

        return configurations;
    }
}
