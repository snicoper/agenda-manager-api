using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Calendars.Errors;

public static class CalendarHolidayErrors
{
    public static Error CreateOverlappingReject =>
        Error.Conflict("Cannot create holiday. Overlapping appointments found.");
}
