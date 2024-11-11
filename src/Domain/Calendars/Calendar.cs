using AgendaManager.Domain.Calendars.Events;
using AgendaManager.Domain.Calendars.Exxceptions;
using AgendaManager.Domain.Calendars.ValueObjects;
using AgendaManager.Domain.Common.Abstractions;

namespace AgendaManager.Domain.Calendars;

public class Calendar : AggregateRoot
{
    internal Calendar(CalendarId id, string name, string description)
    {
        GuardAgainstInvalidName(name);
        GuardAgainstInvalidDescription(description);

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
