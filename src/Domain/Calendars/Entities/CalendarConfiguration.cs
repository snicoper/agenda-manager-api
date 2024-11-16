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

    public static CalendarConfiguration Create(
        CalendarConfigurationId id,
        CalendarId calendarId,
        string category,
        string selectedKey)
    {
        CalendarConfiguration calendarConfiguration = new(id, calendarId, category, selectedKey);

        return calendarConfiguration;
    }
}
