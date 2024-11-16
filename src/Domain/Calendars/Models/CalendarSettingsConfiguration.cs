using AgendaManager.Domain.Calendars.Enums;
using AgendaManager.Domain.Calendars.ValueObjects;

namespace AgendaManager.Domain.Calendars.Models;

public record CalendarSettingsConfiguration(
    IanaTimeZone IanaTimeZone,
    HolidayStrategy HolidayStrategy,
    AppointmentStrategy AppointmentStrategy);
