using FluentValidation;

namespace AgendaManager.Application.Calendars.Queries.GetCalendarHolidays;

public class GetCalendarHolidaysQueryValidator : AbstractValidator<GetCalendarHolidaysQuery>
{
    public GetCalendarHolidaysQueryValidator()
    {
        RuleFor(x => x.CalendarId)
            .NotEmpty().WithMessage("CalendarId is required.");
    }
}
