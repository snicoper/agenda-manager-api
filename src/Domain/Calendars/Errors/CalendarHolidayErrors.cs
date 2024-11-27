using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Calendars.Errors;

public static class CalendarHolidayErrors
{
    public static Error CreateOverlappingReject => Error.Conflict(
        code: "CalendarHolidayErrors.CreateOverlappingReject",
        description: "Cannot create holiday. Overlapping appointments found.");

    public static Error HolidaysOverlap => Error.Conflict(
        code: "CalendarHolidayErrors.HolidaysOverlap",
        description: "Cannot create holiday. Overlapping holidays found.");
}
