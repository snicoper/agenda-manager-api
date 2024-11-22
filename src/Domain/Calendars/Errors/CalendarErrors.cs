using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Calendars.Errors;

public static class CalendarErrors
{
    public static Error CalendarNotFound => Error.NotFound("Calendar not found.");

    public static Error NameAlreadyExists => Error.Validation(nameof(Calendar.Name), "Calendar name already exists.");

    public static Error CannotDeleteCalendarWithAppointments =>
        Error.Conflict("Cannot delete calendar because it has appointments associated.");

    public static Error CannotDeleteCalendarWithServices =>
        Error.Conflict("Cannot delete calendar because it has services associated.");

    public static Error CannotDeleteCalendarWithResources =>
        Error.Conflict("Cannot delete calendar because it has resources associated.");
}
