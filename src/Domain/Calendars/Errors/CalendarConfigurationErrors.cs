using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Calendars.Errors;

public class CalendarConfigurationErrors
{
    public static Error KeyNotFound => Error.Unexpected("Key not found");
}
