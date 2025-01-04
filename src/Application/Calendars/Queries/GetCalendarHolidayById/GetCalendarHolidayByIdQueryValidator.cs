using FluentValidation;

namespace AgendaManager.Application.Calendars.Queries.GetCalendarHolidayById;

public class GetCalendarHolidayByIdQueryValidator : AbstractValidator<GetCalendarHolidayByIdQuery>
{
    public GetCalendarHolidayByIdQueryValidator()
    {
        RuleFor(x => x.CalendarId)
            .NotEmpty().WithMessage("CalendarId is required.");

        RuleFor(x => x.CalendarHolidayId)
            .NotEmpty().WithMessage("CalendarHolidayId is required.");
    }
}
