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

    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public static Calendar Create(CalendarId id, string name, string description)
    {
        // Validations.
        if (string.IsNullOrWhiteSpace(name) || name.Length > 50)
        {
            throw new DomainException("Name cannot be empty and must be less than 50 characters.");
        }

        if (string.IsNullOrWhiteSpace(description) || description.Length > 500)
        {
            throw new DomainException("Description cannot be empty and must be less than 500 characters.");
        }

        Calendar calendar = new(id, name, description);

        calendar.AddDomainEvent(new CalendarCreatedDomainEvent(calendar.Id));

        return calendar;
    }
}
