using AgendaManager.Domain.Calendars.Events;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;
using AgendaManager.Domain.Common.Exceptions;

namespace AgendaManager.Domain.Calendars;

public class Calendar : AggregateRoot
{
    private Calendar()
    {
    }

    private Calendar(CalendarId id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public CalendarId Id { get; } = null!;

    public string Name { get; private set; } = default!;

    public string Description { get; private set; } = default!;

    public static Calendar Create(CalendarId id, string name, string description)
    {
        Validate(name, description);

        Calendar calendar = new(id, name, description);

        calendar.AddDomainEvent(new CalendarCreatedDomainEvent(calendar.Id));

        return calendar;
    }

    public void Update(string name, string description)
    {
        Validate(name, description);

        if (Name == name && Description == description)
        {
            return;
        }

        Name = name;
        Description = description;

        AddDomainEvent(new CalendarUpdatedDomainEvent(Id));
    }

    private static void Validate(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > 50)
        {
            throw new DomainException("Name cannot be empty and must be less than 50 characters.");
        }

        if (string.IsNullOrWhiteSpace(description) || description.Length > 500)
        {
            throw new DomainException("Description cannot be empty and must be less than 500 characters.");
        }
    }
}
