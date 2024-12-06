using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Services;
using AgendaManager.Domain.Services.ValueObjects;

namespace AgendaManager.TestCommon.Factories;

public static class ServiceFactory
{
    public static Service CreateService(
        ServiceId? serviceId = null,
        CalendarId? calendarId = null,
        Duration? duration = null,
        string? name = null,
        string? description = null,
        ColorScheme? colorScheme = null,
        bool? isEnabled = null)
    {
        var service = Service.Create(
            serviceId: serviceId ?? ServiceId.From(Guid.NewGuid()),
            calendarId: calendarId ?? CalendarId.Create(),
            duration: duration ?? Duration.From(TimeSpan.FromHours(1)),
            name: name ?? "Service test name",
            description: description ?? "Service test description",
            colorScheme: colorScheme ?? ColorScheme.From("#ffffff", "#000000"),
            isActive: isEnabled ?? true);

        return service;
    }
}
