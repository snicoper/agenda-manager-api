using AgendaManager.Domain.Calendars.Enums;
using AgendaManager.Domain.Calendars.Models;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.TestCommon.Constants;

namespace AgendaManager.TestCommon.Factories;

public static class CalendarSettingsConfigurationFactory
{
    public static CalendarSettingsConfiguration CreateConfiguration(
        IanaTimeZone? ianaTimeZone = null,
        HolidayCreateStrategy? holidayCreateStrategy = null,
        AppointmentOverlappingStrategy? appointmentOverlappingStrategy = null)
    {
        CalendarSettingsConfiguration configuration = new(
            IanaTimeZone: ianaTimeZone ?? IanaTimeZone.FromIana(IanaTimeZoneConstants.AmericaNewYork),
            HolidayCreateStrategy: holidayCreateStrategy ?? HolidayCreateStrategy.RejectIfOverlapping,
            AppointmentOverlappingStrategy: appointmentOverlappingStrategy ?? AppointmentOverlappingStrategy.RejectIfOverlapping);

        return configuration;
    }
}
