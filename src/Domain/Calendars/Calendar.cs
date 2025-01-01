using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.Events;
using AgendaManager.Domain.Calendars.Exceptions;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.WekDays;

namespace AgendaManager.Domain.Calendars;

public sealed class Calendar : AggregateRoot
{
    private readonly List<CalendarHoliday> _holidays = [];

    private Calendar(
        CalendarId id,
        CalendarSettings settings,
        string name,
        string description,
        bool isActive,
        WeekDays availableDays)
    {
        GuardAgainstInvalidName(name);
        GuardAgainstInvalidDescription(description);

        Id = id;
        Settings = settings;
        SettingsId = settings.Id;
        Name = name;
        Description = description;
        IsActive = isActive;
        AvailableDays = availableDays;
    }

    private Calendar()
    {
    }

    public CalendarId Id { get; } = null!;

    public CalendarSettingsId SettingsId { get; private set; } = null!;

    public CalendarSettings Settings { get; private set; } = null!;

    public string Name { get; private set; } = null!;

    public string Description { get; private set; } = null!;

    public bool IsActive { get; private set; }

    public WeekDays AvailableDays { get; private set; }

    public IReadOnlyList<CalendarHoliday> Holidays => _holidays.AsReadOnly();

    public bool IsAvailableDay(DayOfWeek day)
    {
        return (AvailableDays & (WeekDays)(1 << (int)day)) != 0;
    }

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

    public void AddHoliday(CalendarHoliday holiday)
    {
        _holidays.Add(holiday);

        AddDomainEvent(new CalendarHolidayAddedDomainEvent(Id, holiday.Id));
    }

    public void RemoveHoliday(CalendarHoliday holiday)
    {
        _holidays.Remove(holiday);

        AddDomainEvent(new CalendarHolidayRemovedDomainEvent(Id, holiday.Id));
    }

    public bool UpdateSettings(CalendarSettings settings)
    {
        if (!Settings.HasChanges(settings))
        {
            return false;
        }

        Settings = settings;

        AddDomainEvent(new CalendarSettingsUpdatedDomainEvent(Id));

        return true;
    }

    internal static Calendar Create(
        CalendarId id,
        CalendarSettings settings,
        string name,
        string description,
        bool isActive = true,
        WeekDays availableDays = WeekDays.All)
    {
        Calendar calendar = new(
            id: id,
            settings: settings,
            name: name,
            description: description,
            isActive: isActive,
            availableDays: availableDays);

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

    internal void UpdateAvailableDays(WeekDays availableDays)
    {
        if (AvailableDays == availableDays)
        {
            return;
        }

        AvailableDays = availableDays;

        AddDomainEvent(new CalendarAvailableDaysUpdatedDomainEvent(Id));
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
