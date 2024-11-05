using AgendaManager.Domain.Calendars.Events;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;

namespace AgendaManager.Domain.Calendars;

public class Calendar : AggregateRoot
{
    private Calendar()
    {
    }

    private Calendar(CalendarId id)
    {
        Id = id;
    }

    public CalendarId Id { get; } = null!;

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public static Calendar Create(CalendarId id)
    {
        Calendar calendar = new(id);

        calendar.AddDomainEvent(new CalendarCreatedDomainEvent(calendar.Id));

        return calendar;
    }
}
