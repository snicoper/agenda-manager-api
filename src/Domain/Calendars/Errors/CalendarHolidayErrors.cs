using AgendaManager.Domain.Calendars.Entities;
using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Calendars.Errors;

public static class CalendarHolidayErrors
{
    public static Error CreateOverlappingReject => Error.Conflict(
        code: "CalendarHolidayErrors.CreateOverlappingReject",
        description: "Cannot create a holiday that overlaps with existing appointments.");

    public static Error HolidaysOverlap => Error.Conflict(
        code: "CalendarHolidayErrors.HolidaysOverlap",
        description: "Cannot create a holiday that overlaps with existing holidays.");

    public static Error NameAlreadyExists => Error.Validation(
        code: nameof(CalendarHoliday.Name),
        description: "A holiday with the same name already exists in the calendar.");
}
