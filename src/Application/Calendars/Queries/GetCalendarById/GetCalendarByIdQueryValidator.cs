using FluentValidation;

namespace AgendaManager.Application.Calendars.Queries.GetCalendarById;

public class GetCalendarByIdQueryValidator : AbstractValidator<GetCalendarByIdQuery>
{
    public GetCalendarByIdQueryValidator()
    {
        RuleFor(x => x.CalendarId)
            .NotEmpty().WithMessage("CalendarId is required.");
    }
}
