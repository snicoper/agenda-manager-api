using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Calendars.Errors;

public static class CalendarConfigurationErrors
{
    public static Error KeyNotFound => Error.Unexpected(
        code: "CalendarConfigurationErrors.KeyNotFound",
        description: "Key not found");
}
