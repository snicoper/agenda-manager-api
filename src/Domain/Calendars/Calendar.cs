using AgendaManager.Domain.Calendars.Configurations;
using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.Events;
using AgendaManager.Domain.Calendars.Exceptions;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;

namespace AgendaManager.Domain.Calendars;

public sealed class Calendar : AggregateRoot
{
    private readonly List<CalendarHoliday> _holidays = [];
    private readonly List<CalendarConfiguration> _configurations = [];

    private Calendar(CalendarId id, string name, string description, bool isActive)
    {
        GuardAgainstInvalidName(name);
        GuardAgainstInvalidDescription(description);

        Id = id;
        Name = name;
        Description = description;
        IsActive = isActive;
    }

    private Calendar()
    {
    }

    public CalendarId Id { get; } = null!;

    public string Name { get; private set; } = default!;

    public string Description { get; private set; } = default!;

    public bool IsActive { get; private set; }

    public IReadOnlyCollection<CalendarHoliday> Holidays => _holidays.AsReadOnly();

    public IReadOnlyCollection<CalendarConfiguration> Configurations => _configurations.AsReadOnly();

    public string GetConfigurationValue(string category)
    {
        var config = _configurations.FirstOrDefault(c => c.Category == category)
                     ?? throw new CalendarDomainException($"Configuration not found: {category}");

        return config.SelectedKey;
    }

    public bool AllowsOverlapping()
    {
        return GetConfigurationValue(CalendarConfigurationKeys.Appointments.OverlappingStrategy) ==
               CalendarConfigurationKeys.Appointments.OverlappingOptions.AllowOverlapping;
    }

    public bool RequiresConfirmation()
    {
        return GetConfigurationValue(CalendarConfigurationKeys.Appointments.CreationStrategy) ==
               CalendarConfigurationKeys.Appointments.CreationOptions.RequireConfirmation;
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

    public void AddConfiguration(CalendarConfiguration calendarConfiguration)
    {
        _configurations.Add(calendarConfiguration);

        AddDomainEvent(new CalendarConfigurationAddedDomainEvent(Id, calendarConfiguration.Id));
    }

    public void RemoveConfiguration(CalendarConfiguration calendarConfiguration)
    {
        _configurations.Remove(calendarConfiguration);

        AddDomainEvent(new CalendarConfigurationRemovedDomainEvent(Id, calendarConfiguration.Id));
    }

    public void UpdateConfiguration(CalendarConfigurationId configurationId, string category, string selectedKey)
    {
        var calendarConfiguration = _configurations.FirstOrDefault(x => x.Id == configurationId);

        if (calendarConfiguration is null)
        {
            throw new CalendarConfigurationDomainException("Invalid calendar configuration option.");
        }

        if (calendarConfiguration.Update(category, selectedKey))
        {
            AddDomainEvent(new CalendarConfigurationUpdatedDomainEvent(Id, calendarConfiguration.Id));
        }
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

    internal static Calendar Create(CalendarId id, string name, string description, bool active = true)
    {
        Calendar calendar = new(id, name, description, active);

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
