using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.ValueObjects.ColorScheme;
using AgendaManager.Domain.ResourceTypes;
using AgendaManager.Domain.Services.Events;
using AgendaManager.Domain.Services.Exceptions;
using AgendaManager.Domain.Services.ValueObjects;

namespace AgendaManager.Domain.Services;

public sealed class Service : AggregateRoot
{
    private readonly List<ResourceType> _resourceTypes = [];

    private Service()
    {
    }

    private Service(
        ServiceId serviceId,
        CalendarId calendarId,
        TimeSpan duration,
        string name,
        string description,
        ColorScheme colorScheme,
        bool isActive)
    {
        Id = serviceId;
        CalendarId = calendarId;
        Duration = duration;
        Name = name;
        Description = description;
        ColorScheme = colorScheme;
        IsActive = isActive;
    }

    public ServiceId Id { get; } = null!;

    public CalendarId CalendarId { get; private set; } = null!;

    public Calendar Calendar { get; private set; } = null!;

    public TimeSpan Duration { get; private set; } = TimeSpan.Zero;

    public string Name { get; private set; } = null!;

    public string Description { get; private set; } = null!;

    public ColorScheme ColorScheme { get; private set; } = null!;

    public bool IsActive { get; private set; } = true;

    public IReadOnlyList<ResourceType> ResourceTypes => _resourceTypes.AsReadOnly();

    public void AddResourceType(ResourceType resourceType)
    {
        _resourceTypes.Add(resourceType);

        AddDomainEvent(new ResourceTypeAddedToServiceDomainEvent(Id, resourceType.Id));
    }

    public void RemoveResourceType(ResourceType resourceType)
    {
        _resourceTypes.Remove(resourceType);

        AddDomainEvent(new ResourceTypeRemovedFromServiceDomainEvent(Id, resourceType.Id));
    }

    internal static Service Create(
        ServiceId serviceId,
        CalendarId calendarId,
        TimeSpan duration,
        string name,
        string description,
        ColorScheme colorScheme,
        bool isActive = true)
    {
        GuardAgainstInvalidDuration(duration);
        GuardAgainstInvalidName(name);
        GuardAgainstInvalidDescription(description);

        Service service = new(serviceId, calendarId, duration, name, description, colorScheme, isActive);

        service.AddDomainEvent(new ServiceCreatedDomainEvent(service.Id));

        return service;
    }

    internal bool Update(
        TimeSpan duration,
        string name,
        string description,
        ColorScheme colorScheme)
    {
        GuardAgainstInvalidDuration(duration);
        GuardAgainstInvalidName(name);
        GuardAgainstInvalidDescription(description);

        if (!HasChanges(duration, name, description, colorScheme))
        {
            return false;
        }

        Duration = duration;
        Name = name;
        Description = description;
        ColorScheme = colorScheme;

        AddDomainEvent(new ServiceUpdatedDomainEvent(Id));

        return true;
    }

    private static void GuardAgainstInvalidDuration(TimeSpan duration)
    {
        ArgumentNullException.ThrowIfNull(duration);

        if (duration <= TimeSpan.Zero)
        {
            throw new ServiceDomainException("Service duration must be greater than zero.");
        }
    }

    private static void GuardAgainstInvalidName(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        if (string.IsNullOrWhiteSpace(name) || name.Length > 50)
        {
            throw new ServiceDomainException("Service name must be between 1 and 50 characters.");
        }
    }

    private static void GuardAgainstInvalidDescription(string description)
    {
        ArgumentNullException.ThrowIfNull(description);

        if (string.IsNullOrWhiteSpace(description) || description.Length > 500)
        {
            throw new ServiceDomainException("Service description must be between 1 and 500 characters.");
        }
    }

    private bool HasChanges(TimeSpan duration, string name, string description, ColorScheme colorScheme)
    {
        return !(Duration == duration
                 && Name == name
                 && Description == description
                 && ColorScheme == colorScheme);
    }
}
