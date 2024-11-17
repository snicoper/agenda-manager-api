using AgendaManager.Domain.Calendars.Configurations;
using AgendaManager.Domain.Calendars.Exceptions;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;

namespace AgendaManager.Domain.Calendars.Entities;

public sealed class CalendarConfiguration : AuditableEntity
{
    private CalendarConfiguration()
    {
    }

    private CalendarConfiguration(
        CalendarConfigurationId id,
        CalendarId calendarId,
        string category,
        string selectedKey)
    {
        Id = id;
        CalendarId = calendarId;
        Category = category;
        SelectedKey = selectedKey;
    }

    public CalendarConfigurationId Id { get; } = null!;

    public CalendarId CalendarId { get; private set; } = null!;

    public Calendar Calendar { get; private set; } = null!;

    public string Category { get; private set; } = default!;

    public string SelectedKey { get; private set; } = default!;

    internal static CalendarConfiguration Create(
        CalendarConfigurationId id,
        CalendarId calendarId,
        string category,
        string selectedKey)
    {
        GuardAgainstInvalidCategory(category);
        GuardAgainstInvalidSelectedKey(selectedKey);
        GuardAgainstInvalidConfiguration(category, selectedKey);

        return new CalendarConfiguration(id, calendarId, category, selectedKey);
    }

    internal bool Update(string category, string selectedKey)
    {
        if (!HasChanges(category, selectedKey))
        {
            return false;
        }

        GuardAgainstInvalidCategory(category);
        GuardAgainstInvalidSelectedKey(selectedKey);
        GuardAgainstInvalidConfiguration(category, selectedKey);

        Category = category;
        SelectedKey = selectedKey;

        return true;
    }

    internal bool IsUnitValueConfiguration()
    {
        return CalendarConfigurationKeys.Metadata.Options[Category].IsUnitValue;
    }

    private static void GuardAgainstInvalidCategory(string category)
    {
        ArgumentNullException.ThrowIfNull(category);

        if (string.IsNullOrWhiteSpace(category) || category.Length > 100)
        {
            throw new CalendarConfigurationDomainException("The category must be between 0 and 100 characters.");
        }
    }

    private static void GuardAgainstInvalidSelectedKey(string selectedKey)
    {
        ArgumentNullException.ThrowIfNull(selectedKey);

        if (string.IsNullOrWhiteSpace(selectedKey) || selectedKey.Length > 100)
        {
            throw new CalendarConfigurationDomainException("The selected key must be between 0 and 100 characters.");
        }
    }

    private static void GuardAgainstInvalidConfiguration(string category, string selectedKey)
    {
        if (!CalendarConfigurationKeys.Metadata.IsValidConfiguration(category, selectedKey))
        {
            throw new CalendarConfigurationDomainException(
                $"Invalid configuration combination: {category} - {selectedKey}");
        }
    }

    private bool HasChanges(string category, string selectedKey)
    {
        return !(category == Category && selectedKey == SelectedKey);
    }
}
