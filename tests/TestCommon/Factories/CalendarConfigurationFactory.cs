using AgendaManager.Domain.Calendars.Configurations;
using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.ValueObjects;

namespace AgendaManager.TestCommon.Factories;

public static class CalendarConfigurationFactory
{
    public static CalendarConfiguration CreateCalendarConfiguration(
        CalendarConfigurationId? id = null,
        CalendarId? calendarId = null,
        string? category = null,
        string? selectedKey = null)
    {
        var calendarConfiguration = CalendarConfiguration.Create(
            id: id ?? CalendarConfigurationId.Create(),
            calendarId: calendarId ?? CalendarId.Create(),
            category: category ?? CalendarConfigurationKeys.Appointments.ConfirmationStrategy,
            selectedKey: selectedKey ?? CalendarConfigurationKeys.Appointments.ConfirmationOptions.RequireConfirmation);

        return calendarConfiguration;
    }

    public static CalendarConfiguration CreateCalendarConfigurationUnitValue()
    {
        var calendarConfiguration = CreateCalendarConfiguration(
            category: CalendarConfigurationKeys.TimeZone.Category,
            selectedKey: "Europe/Madrid");

        return calendarConfiguration;
    }
}
