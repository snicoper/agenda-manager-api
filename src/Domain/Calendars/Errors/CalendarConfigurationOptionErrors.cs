using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Calendars.Errors;

public static class CalendarConfigurationOptionErrors
{
    public static Error NoDefaultConfigurationsFound => Error.Unexpected("Default configurations not found");
}
