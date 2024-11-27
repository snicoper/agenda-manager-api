using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Calendars.Errors;

public static class CalendarErrors
{
    public static Error CalendarNotFound => Error.NotFound(
        code: "CalendarErrors.CalendarNotFound",
        description: "The calendar was not found.");

    public static Error NameAlreadyExists => Error.Validation(
        code: nameof(Calendar.Name),
        description: "The calendar name already exists.");

    public static Error CannotDeleteCalendarWithAppointments => Error.Conflict(
        code: "CalendarErrors.CannotDeleteCalendarWithAppointments",
        description: "The calendar cannot be deleted because it has appointments.");

    public static Error CannotDeleteCalendarWithServices => Error.Conflict(
        code: "CalendarErrors.CannotDeleteCalendarWithServices",
        description: "The calendar cannot be deleted because it has services.");

    public static Error CannotDeleteCalendarWithResources => Error.Conflict(
        code: "CalendarErrors.CannotDeleteCalendarWithResources",
        description: "The calendar cannot be deleted because it has resources.");
}
