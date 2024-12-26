using FluentValidation;

namespace AgendaManager.Application.Calendars.Queries.GetCalendarSettings;

public class GetCalendarSettingsQueryValidator : AbstractValidator<GetCalendarSettingsQuery>
{
    public GetCalendarSettingsQueryValidator()
    {
        RuleFor(x => x.CalendarId)
            .NotEmpty().WithMessage("CalendarId is required.");
    }
}
