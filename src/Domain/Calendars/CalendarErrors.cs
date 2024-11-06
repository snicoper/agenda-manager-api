using AgendaManager.Domain.Common.Responses;

namespace AgendaManager.Domain.Calendars;

public static class CalendarErrors
{
    public static Error InvalidFormatName =>
        Error.Validation(nameof(Calendar.Name), "Name cannot be empty and must be less than 50 characters.");

    public static Error InvalidFormatDescription =>
        Error.Validation(
            nameof(Calendar.Description),
            "Description cannot be empty and must be less than 500 characters.");

    public static Error NameAlreadyExists => Error.Validation(nameof(Calendar.Name), "Calendar name already exists");

    public static Error DescriptionAlreadyExists => Error.Validation(
        nameof(Calendar.Description),
        "Calendar description already exists");
}
