using AgendaManager.Domain.Calendars.Events;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;

namespace AgendaManager.Domain.Calendars.Entities;

public sealed class CalendarConfigurationOption : AggregateRoot
{
    private CalendarConfigurationOption()
    {
    }

    private CalendarConfigurationOption(
        CalandarConfigurationOptionId optionId,
        string category,
        string key,
        string description,
        bool defaultValue)
    {
        OptionId = optionId;
        Category = category;
        Key = key;
        Description = description;
        DefaultValue = defaultValue;
    }

    public CalandarConfigurationOptionId OptionId { get; } = null!;

    public string Category { get; private set; } = default!;

    public string Key { get; private set; } = default!;

    public bool DefaultValue { get; private set; }

    public string Description { get; private set; } = default!;

    public static CalendarConfigurationOption Create(
        CalandarConfigurationOptionId optionId,
        string category,
        string key,
        string description,
        bool defaultValue = false)
    {
        CalendarConfigurationOption calendarConfigurationOption = new(
            optionId,
            category,
            key,
            description,
            defaultValue);

        calendarConfigurationOption.AddDomainEvent(new CalendarConfigurationOptionCreatedDomainEvent(optionId));

        return calendarConfigurationOption;
    }
}
