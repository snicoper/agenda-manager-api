using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.Responses;
using AgendaManager.Domain.Common.ValueObjects;
using AgendaManager.Domain.Common.WekDays;
using AgendaManager.Domain.Resources.Entities;
using AgendaManager.Domain.Resources.Errors;
using AgendaManager.Domain.Resources.Events;
using AgendaManager.Domain.Resources.Exceptions;
using AgendaManager.Domain.Resources.ValueObjects;
using AgendaManager.Domain.ResourceTypes;
using AgendaManager.Domain.ResourceTypes.ValueObjects;
using AgendaManager.Domain.Users;
using AgendaManager.Domain.Users.ValueObjects;

namespace AgendaManager.Domain.Resources;

public sealed class Resource : AggregateRoot
{
    private readonly List<ResourceSchedule> _schedules = [];

    private Resource()
    {
    }

    private Resource(
        ResourceId id,
        UserId? userId,
        CalendarId calendarId,
        ResourceTypeId typeId,
        string name,
        string description,
        ColorScheme colorScheme,
        bool isActive)
    {
        GuardAgainstInvalidName(name);
        GuardAgainstInvalidDescription(description);

        Id = id;
        UserId = userId;
        CalendarId = calendarId;
        TypeId = typeId;
        Name = name;
        Description = description;
        ColorScheme = colorScheme;
        IsActive = isActive;
    }

    public ResourceId Id { get; } = null!;

    public UserId? UserId { get; }

    public User? User { get; private set; }

    public CalendarId CalendarId { get; } = default!;

    public Calendar Calendar { get; private set; } = null!;

    public ResourceTypeId TypeId { get; private set; } = null!;

    public ResourceType Type { get; private set; } = null!;

    public string Name { get; private set; } = default!;

    public string Description { get; private set; } = default!;

    public ColorScheme ColorScheme { get; private set; } = null!;

    public bool IsActive { get; private set; } = true;

    public string? DeactivationReason { get; private set; }

    public IReadOnlyList<ResourceSchedule> Schedules => _schedules.AsReadOnly();

    public void AddSchedule(ResourceSchedule schedule)
    {
        _schedules.Add(schedule);

        AddDomainEvent(new ResourceScheduleCreatedDomainEvent(schedule.Id));
    }

    public void RemoveSchedule(ResourceSchedule schedule)
    {
        _schedules.Remove(schedule);

        AddDomainEvent(new ResourceScheduleRemovedDomainEvent(schedule.Id));
    }

    public void Activate()
    {
        if (IsActive)
        {
            return;
        }

        GuardAgainstInvalidDeactivationReason(Name);

        IsActive = true;
        DeactivationReason = null;

        AddDomainEvent(new ResourceActivatedDomainEvent(Id));
    }

    public void Deactivate(string reason)
    {
        if (!IsActive)
        {
            return;
        }

        IsActive = false;
        DeactivationReason = reason;

        AddDomainEvent(new ResourceDeactivatedDomainEvent(Id));
    }

    public Result UpdateSchedule(
        ResourceScheduleId scheduleId,
        Period period,
        string name,
        string? description,
        WeekDays availableDays)
    {
        var schedule = _schedules.FirstOrDefault(r => r.Id == scheduleId);

        if (schedule is null)
        {
            return ResourceScheduleErrors.NotFound;
        }

        if (schedule.Update(period, name, description, availableDays))
        {
            AddDomainEvent(new ResourceScheduleUpdatedDomainEvent(Id, scheduleId));
        }

        return Result.Success();
    }

    public Result ActiveSchedule(ResourceScheduleId scheduleId)
    {
        var schedule = _schedules.FirstOrDefault(r => r.Id == scheduleId);

        if (schedule is null)
        {
            return ResourceScheduleErrors.NotFound;
        }

        if (schedule.Activate())
        {
            AddDomainEvent(new ResourceScheduleActivatedDomainEvent(scheduleId));
        }

        return Result.Success();
    }

    public Result DeactivateSchedule(ResourceScheduleId scheduleId)
    {
        var schedule = _schedules.FirstOrDefault(r => r.Id == scheduleId);

        if (schedule is null)
        {
            return ResourceScheduleErrors.NotFound;
        }

        if (schedule.Deactivate())
        {
            AddDomainEvent(new ResourceScheduleDeactivatedDomainEvent(scheduleId));
        }

        return Result.Success();
    }

    internal static Resource Create(
        ResourceId id,
        UserId? userId,
        CalendarId calendarId,
        ResourceTypeId typeId,
        string name,
        string description,
        ColorScheme colorScheme,
        bool isActive)
    {
        Resource resource = new(id, userId, calendarId, typeId, name, description, colorScheme, isActive);

        resource.AddDomainEvent(new ResourceCreatedDomainEvent(resource.Id));

        return resource;
    }

    internal void Update(string name, string description, ColorScheme colorScheme)
    {
        GuardAgainstInvalidName(name);
        GuardAgainstInvalidDescription(description);

        if (Name == name && Description == description && ColorScheme == colorScheme)
        {
            return;
        }

        Name = name;
        Description = description;
        ColorScheme = colorScheme;

        AddDomainEvent(new ResourceUpdatedDomainEvent(Id));
    }

    private static void GuardAgainstInvalidName(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        if (string.IsNullOrWhiteSpace(name) || name.Length > 50)
        {
            throw new ResourceDomainException("Resource name must be between 1 and 50 characters long.");
        }
    }

    private static void GuardAgainstInvalidDescription(string description)
    {
        ArgumentNullException.ThrowIfNull(description);

        if (string.IsNullOrWhiteSpace(description) || description.Length > 500)
        {
            throw new ResourceDomainException("Resource description must be between 1 and 500 characters long.");
        }
    }

    private static void GuardAgainstInvalidDeactivationReason(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        if (string.IsNullOrWhiteSpace(name) || name.Length > 256)
        {
            throw new ResourceDomainException(
                "Resource deactivation reason must be between 1 and 256 characters long.");
        }
    }
}
