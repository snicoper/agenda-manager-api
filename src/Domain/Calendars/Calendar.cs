using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.Events;
using AgendaManager.Domain.Calendars.Exceptions;
using AgendaManager.Domain.Calendars.Models;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;

namespace AgendaManager.Domain.Calendars;

public sealed class Calendar : AggregateRoot
{
    private readonly List<CalendarHoliday> _holidays = [];

    private Calendar(
        CalendarId id,
        string name,
        string description,
        CalendarSettings calendarSettings,
        bool isActive)
    {
        GuardAgainstInvalidName(name);
        GuardAgainstInvalidDescription(description);

        Id = id;
        SettingsId = CalendarSettingsId.Create();
        Settings = calendarSettings;
        Name = name;
        Description = description;
        IsActive = isActive;
    }

    private Calendar()
    {
    }

    public CalendarId Id { get; } = null!;

    public CalendarSettingsId SettingsId { get; } = null!;

    public CalendarSettings Settings { get; } = null!;

    public string Name { get; private set; } = default!;

    public string Description { get; private set; } = default!;

    public bool IsActive { get; private set; }

    public IReadOnlyCollection<CalendarHoliday> Holidays => _holidays.AsReadOnly();

    public void Activate()
    {
        if (IsActive)
        {
            return;
        }

        IsActive = true;

        AddDomainEvent(new CalendarActivatedDomainEvent(Id));
    }

    public void Deactivate()
    {
        if (!IsActive)
        {
            return;
        }

        IsActive = false;
        AddDomainEvent(new CalendarDeactivatedDomainEvent(Id));
    }

    public void AddHoliday(CalendarHoliday calendarHoliday)
    {
        _holidays.Add(calendarHoliday);

        AddDomainEvent(new CalendarHolidayAddedDomainEvent(Id, calendarHoliday.Id));
    }

    public void RemoveHoliday(CalendarHoliday calendarHoliday)
    {
        _holidays.Remove(calendarHoliday);

        AddDomainEvent(new CalendarHolidayRemovedDomainEvent(Id, calendarHoliday.Id));
    }

    public void UpdateSettings(CalendarSettingsConfiguration configuration)
    {
        if (!Settings.HasChanges(configuration))
        {
            return;
        }

        Settings.Update(configuration);

        AddDomainEvent(new CalendarSettingsUpdatedDomainEvent(SettingsId, Id));
    }

    internal static Calendar Create(
        CalendarId id,
        string name,
        string description,
        CalendarSettings calendarSettings,
        bool active = true)
    {
        Calendar calendar = new(id, name, description, calendarSettings, active);

        calendar.AddDomainEvent(new CalendarCreatedDomainEvent(id));

        return calendar;
    }

    internal void Update(string name, string description)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(description);

        GuardAgainstInvalidName(name);
        GuardAgainstInvalidDescription(description);

        if (Name == name && Description == description)
        {
            return;
        }

        Name = name;
        Description = description;

        AddDomainEvent(new CalendarUpdatedDomainEvent(Id));
    }

    private static void GuardAgainstInvalidName(string name)
    {
        ArgumentNullException.ThrowIfNull(name);

        if (string.IsNullOrWhiteSpace(name) || name.Length > 50)
        {
            throw new CalendarDomainException("Name is invalid or exceeds length of 50 characters.");
        }
    }

    private static void GuardAgainstInvalidDescription(string description)
    {
        ArgumentNullException.ThrowIfNull(description);

        if (string.IsNullOrWhiteSpace(description) || description.Length > 500)
        {
            throw new CalendarDomainException("Description is invalid or exceeds length of 500 characters.");
        }
    }
}
