using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;

namespace AgendaManager.Domain.Calendars.Entities;

public sealed class CalendarConfiguration : AuditableEntity
{
    private CalendarConfiguration()
    {
    }

    private CalendarConfiguration(
        CalendarConfigurationId calendarConfigurationId,
        CalendarId calendarId,
        string category,
        string selectedKey)
    {
        Id = calendarConfigurationId;
        CalendarId = calendarId;
        Category = category;
        SelectedKey = selectedKey;
    }

    public CalendarConfigurationId Id { get; } = null!;

    public CalendarId CalendarId { get; private set; } = null!;

    public Calendar Settings { get; private set; } = null!;

    public string Category { get; private set; } = default!;

    public string SelectedKey { get; private set; } = default!;

    public static CalendarConfiguration Create(
        CalendarConfigurationId calendarConfigurationId,
        CalendarId calendarId,
        string category,
        string selectedKey)
    {
        CalendarConfiguration calendarConfiguration = new(calendarConfigurationId, calendarId, category, selectedKey);

        return calendarConfiguration;
    }
}
