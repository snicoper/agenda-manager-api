using AgendaManager.Domain.Calendars;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.ValueObjects.Period;
using AgendaManager.Domain.Common.WekDays;
using AgendaManager.Domain.Resources.Enums;
using AgendaManager.Domain.Resources.Exceptions;
using AgendaManager.Domain.Resources.ValueObjects;

namespace AgendaManager.Domain.Resources.Entities;

public sealed class ResourceSchedule : AggregateRoot
{
    private ResourceSchedule()
    {
    }

    private ResourceSchedule(
        ResourceScheduleId resourceScheduleId,
        ResourceId resourceId,
        CalendarId calendarId,
        Period period,
        ResourceScheduleType type,
        WeekDays availableDays,
        string name,
        string? description,
        bool isActive)
    {
        Id = resourceScheduleId;
        ResourceId = resourceId;
        CalendarId = calendarId;
        Period = period;
        Type = type;
        AvailableDays = availableDays;
        Name = name;
        Description = description;
        IsActive = isActive;
    }

    public ResourceScheduleId Id { get; } = null!;

    public ResourceId ResourceId { get; private set; } = null!;

    public Resource Resource { get; private set; } = null!;

    public CalendarId CalendarId { get; private set; } = null!;

    public Calendar Calendar { get; private set; } = null!;

    public Period Period { get; private set; } = null!;

    public ResourceScheduleType Type { get; private set; } = ResourceScheduleType.Available;

    public WeekDays AvailableDays { get; private set; } = WeekDays.None;

    public string Name { get; private set; } = default!;

    public string? Description { get; private set; }

    public bool IsActive { get; private set; }

    public static ResourceSchedule Create(
        ResourceScheduleId resourceScheduleId,
        ResourceId resourceId,
        CalendarId calendarId,
        Period period,
        ResourceScheduleType type,
        WeekDays availableDays,
        string name,
        string? description,
        bool isActive = true)
    {
        GuardAgainstInvalidName(name);
        GuardAgainstInvalidDescription(description);

        ResourceSchedule resourceSchedule = new(
            resourceScheduleId,
            resourceId,
            calendarId,
            period,
            type,
            availableDays,
            name,
            description,
            isActive);

        return resourceSchedule;
    }

    internal bool Update(Period period, string name, string? description, WeekDays availableDays)
    {
        if (!HasChanges(period, name, description, availableDays))
        {
            return false;
        }

        GuardAgainstInvalidName(name);
        GuardAgainstInvalidDescription(description);

        Period = period;
        Name = name;
        Description = description;
        AvailableDays = availableDays;

        return true;
    }

    internal bool Activate()
    {
        if (IsActive)
        {
            return false;
        }

        IsActive = true;

        return true;
    }

    internal bool Deactivate()
    {
        if (!IsActive)
        {
            return false;
        }

        IsActive = false;

        return true;
    }

    private static void GuardAgainstInvalidName(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        if (!string.IsNullOrWhiteSpace(name) && name.Length > 50)
        {
            throw new ResourceScheduleDomainException("Name is empty or exceeds length of 50 characters.");
        }
    }

    private static void GuardAgainstInvalidDescription(string? description)
    {
        if (!string.IsNullOrWhiteSpace(description) && description.Length > 500)
        {
            throw new ResourceScheduleDomainException("Description exceeds length of 500 characters.");
        }
    }

    private bool HasChanges(Period period, string name, string? description, WeekDays availableDays)
    {
        return !(Period.Equals(period)
                 && Name == name
                 && Description == description
                 && AvailableDays == availableDays);
    }
}
