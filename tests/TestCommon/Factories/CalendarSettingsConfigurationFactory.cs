using AgendaManager.Domain.Calendars.Enums;
using AgendaManager.Domain.Calendars.Models;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.TestCommon.Constants;

namespace AgendaManager.TestCommon.Factories;

public static class CalendarSettingsConfigurationFactory
{
    public static CalendarSettingsConfiguration CreateConfiguration(
        IanaTimeZone? ianaTimeZone = null,
        HolidayStrategy? holidayStrategy = null,
        AppointmentOverlappingStrategy? appointmentStrategy = null)
    {
        CalendarSettingsConfiguration configuration = new(
            IanaTimeZone: ianaTimeZone ?? IanaTimeZone.FromIana(IanaTimeZoneConstants.AmericaNewYork),
            HolidayStrategy: holidayStrategy ?? HolidayStrategy.RejectIfOverlapping,
            AppointmentOverlappingStrategy: appointmentStrategy ?? AppointmentOverlappingStrategy.RejectIfOverlapping);

        return configuration;
    }
}
