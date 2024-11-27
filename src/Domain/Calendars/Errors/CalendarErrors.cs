using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Calendars.Errors;

public static class CalendarErrors
{
    public static Error CalendarNotFound => Error.NotFound(
        code: "CalendarErrors.CalendarNotFound",
        description: "Calendar not found.");

    public static Error NameAlreadyExists => Error.Validation(
        code: nameof(Calendar.Name),
        description: "Calendar name already exists.");

    public static Error CannotDeleteCalendarWithAppointments => Error.Conflict(
        code: "CalendarErrors.CannotDeleteCalendarWithAppointments",
        description: "Cannot delete calendar because it has appointments associated.");

    public static Error CannotDeleteCalendarWithServices => Error.Conflict(
        code: "CalendarErrors.CannotDeleteCalendarWithServices",
        description: "Cannot delete calendar because it has services associated.");

    public static Error CannotDeleteCalendarWithResources => Error.Conflict(
        code: "CalendarErrors.CannotDeleteCalendarWithResources",
        description: "Cannot delete calendar because it has resources associated.");
}
