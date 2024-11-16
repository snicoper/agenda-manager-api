using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Calendars.Errors;

public static class CalendarErrors
{
    public static Error CalendarNotFound => Error.NotFound("Calendar not found.");

    public static Error NameAlreadyExists => Error.Validation(nameof(Calendar.Name), "Calendar name already exists");
}
