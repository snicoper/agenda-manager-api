using AgendaManager.Domain.Calendars.Events;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;

namespace AgendaManager.Domain.Calendars;

public class Calendar : AggregateRoot
{
    internal Calendar(CalendarId id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;

        AddDomainEvent(new CalendarCreatedDomainEvent(id));
    }

    private Calendar()
    {
    }

    public CalendarId Id { get; } = null!;

    public string Name { get; private set; } = default!;

    public string Description { get; private set; } = default!;

    internal void Update(string name, string description)
    {
        if (Name == name && Description == description)
        {
            return;
        }

        Name = name;
        Description = description;

        AddDomainEvent(new CalendarUpdatedDomainEvent(Id));
    }
}
