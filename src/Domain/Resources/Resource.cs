using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.ValueObjects.ColorScheme;
using AgendaManager.Domain.Resources.Events;
using AgendaManager.Domain.Resources.ValueObjects;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Resources;

public sealed class Resource : AggregateRoot
{
    private Resource()
    {
    }

    private Resource(ResourceId id)
    {
        Id = id;
    }

    public ResourceId Id { get; } = null!;

    public UserId? UserId { get; private set; }

    public User? User { get; private set; }

    public CalendarId CalendarId { get; private set; } = default!;

    public Calendar Calendar { get; private set; } = null!;

    public ResourceTypeId TypeId { get; private set; } = null!;

    public ResourceType Type { get; private set; } = null!;

    public string Name { get; private set; } = default!;

    public string Description { get; private set; } = default!;

    public ColorScheme ColorScheme { get; private set; } = null!;

    public static Resource Create(ResourceId id)
    {
        Resource resource = new(id);

        resource.AddDomainEvent(new ResourceCreatedDomainEvent(resource.Id));

        return resource;
    }
}
