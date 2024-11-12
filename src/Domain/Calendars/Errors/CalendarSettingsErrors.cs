using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Calendars.Errors;

public static class CalendarSettingsErrors
{
    public static Error CalendarSettingsNotFound => Error.NotFound("Calendar settings not found.");
}
