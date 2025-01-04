using FluentValidation;

namespace AgendaManager.Application.Calendars.Queries.GetCalendarHolidaysInYear;

public class GetCalendarHolidaysInYearQueryValidator : AbstractValidator<GetCalendarHolidaysInYearQuery>
{
    public GetCalendarHolidaysInYearQueryValidator()
    {
        RuleFor(x => x.CalendarId)
            .NotEmpty().WithMessage("CalendarId is required.");
    }
}
