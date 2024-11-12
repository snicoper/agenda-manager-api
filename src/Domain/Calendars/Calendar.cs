﻿using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Calendars.Events;
using AgendaManager.Domain.Calendars.Exceptions;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;

namespace AgendaManager.Domain.Calendars;

public sealed class Calendar : AggregateRoot
{
    private readonly List<CalendarHoliday> _holidays = [];

    private Calendar(
        CalendarId id,
        CalendarSettings settings,
        string name,
        string description,
        bool isActive)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(description);

        GuardAgainstInvalidName(name);
        GuardAgainstInvalidDescription(description);

        Id = id;
        Settings = settings;
        SettingsId = settings.Id;
        Name = name;
        Description = description;
        IsActive = isActive;
    }

    private Calendar()
    {
    }

    public CalendarId Id { get; } = null!;

    public CalendarSettingsId SettingsId { get; private set; } = null!;

    public CalendarSettings Settings { get; private set; } = null!;

    public string Name { get; private set; } = default!;

    public string Description { get; private set; } = default!;

    public bool IsActive { get; private set; }

    public IReadOnlyCollection<CalendarHoliday> Holidays => _holidays.AsReadOnly();

    public void ChangeActiveStatus(bool isActive)
    {
        if (IsActive == isActive)
        {
            return;
        }

        IsActive = isActive;
        AddDomainEvent(new CalendarActiveStatusChangedDomainEvent(Id, IsActive));
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

    internal static Calendar Create(
        CalendarId id,
        CalendarSettings settings,
        string name,
        string description,
        bool active = true)
    {
        Calendar calendar = new(id, settings, name, description, active);

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
        if (string.IsNullOrWhiteSpace(name) || name.Length > 50)
        {
            throw new CalendarDomainException("Name is invalid or exceeds length of 50 characters.");
        }
    }

    private static void GuardAgainstInvalidDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description) || description.Length > 500)
        {
            throw new CalendarDomainException("Description is invalid or exceeds length of 500 characters.");
        }
    }
}
