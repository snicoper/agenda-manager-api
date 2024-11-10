using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.ValueObjects.ColorScheme;
using AgendaManager.Domain.Resources;
using AgendaManager.Domain.Services.Events;
using AgendaManager.Domain.Services.ValueObjects;

namespace AgendaManager.Domain.Services;

public class Service : AggregateRoot
{
    private readonly List<ResourceType> _resourceTypes = [];

    private Service()
    {
    }

    private Service(ServiceId serviceId)
    {
        Id = serviceId;
    }

    public ServiceId Id { get; } = null!;

    public CalendarId CalendarId { get; private set; } = null!;

    public Calendar Calendar { get; private set; } = null!;

    public TimeSpan Duration { get; private set; } = TimeSpan.Zero;

    public string Name { get; private set; } = null!;

    public string Description { get; private set; } = null!;

    public ColorScheme ColorScheme { get; private set; } = null!;

    public IReadOnlyCollection<ResourceType> ResourceTypes => _resourceTypes.AsReadOnly();

    public static Service Create(ServiceId serviceId)
    {
        Service service = new(serviceId);

        service.AddDomainEvent(new ServiceCreatedDomainEvent(service.Id));

        return service;
    }
}
