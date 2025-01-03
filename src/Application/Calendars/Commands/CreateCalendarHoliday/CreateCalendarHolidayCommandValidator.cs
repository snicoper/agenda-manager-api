using FluentValidation;

namespace AgendaManager.Application.Calendars.Commands.CreateCalendarHoliday;

public class CreateCalendarHolidayCommandValidator : AbstractValidator<CreateCalendarHolidayCommand>
{
    public CreateCalendarHolidayCommandValidator()
    {
        RuleFor(v => v.CalendarId)
            .NotEmpty().WithMessage("CalendarId is required.");

        RuleFor(v => v.Name)
            .MaximumLength(50).WithMessage("Name must not exceed 50 characters.")
            .NotEmpty().WithMessage("Name is required.");

        RuleFor(v => v.Start)
            .NotEmpty().WithMessage("StartDate is required.");

        RuleFor(v => v.End)
            .NotEmpty().WithMessage("EndDate is required.")
            .GreaterThanOrEqualTo(v => v.Start).WithMessage("EndDate must be greater than or equal to StartDate.");
    }
}
