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
        CalendarConfiguration configuration = new(id, calendarId, category, selectedKey);

        return configuration;
    }

    internal void Update(string category, string selectedKey)
    {
        Category = category;
        SelectedKey = selectedKey;
    }
}
