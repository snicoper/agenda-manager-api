using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.ResourceManagement.Resources;
using AgendaManager.Domain.ResourceManagement.Resources.ValueObjects;
using AgendaManager.Domain.ResourceManagement.ResourceTypes.ValueObjects;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.TestCommon.Factories;

public static class ResourceFactory
{
    public static Resource CreateResource(
        ResourceId? resourceId = null,
        UserId? userId = null,
        CalendarId? calendarId = null,
        ResourceTypeId? resourceTypeId = null,
        string? name = null,
        string? description = null,
        ColorScheme? colorScheme = null,
        bool? isActive = null)
    {
        var resource = Resource.Create(
            id: resourceId ?? ResourceId.Create(),
            userId: userId ?? UserId.Create(),
            calendarId: calendarId ?? CalendarId.Create(),
            typeId: resourceTypeId ?? ResourceTypeId.Create(),
            name: name ?? "Default Name",
            description: description ?? "Default Description",
            colorScheme: colorScheme ?? ColorScheme.From("#ffffff", "#000000"),
            isActive: isActive ?? true);

        return resource;
    }
}
