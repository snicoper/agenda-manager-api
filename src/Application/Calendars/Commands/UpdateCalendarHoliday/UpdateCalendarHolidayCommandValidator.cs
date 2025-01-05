using FluentValidation;

namespace AgendaManager.Application.Calendars.Commands.UpdateCalendarHoliday;

public class UpdateCalendarHolidayCommandValidator : AbstractValidator<UpdateCalendarHolidayCommand>
{
    public UpdateCalendarHolidayCommandValidator()
    {
        RuleFor(v => v.CalendarId)
            .NotEmpty().WithMessage("CalendarId is required.");

        RuleFor(v => v.CalendarHolidayId)
            .NotEmpty().WithMessage("CalendarHolidayId is required.");

        RuleFor(v => v.Name)
            .MaximumLength(50).WithMessage("Name must not exceed 50 characters.")
            .NotEmpty().WithMessage("Name is required.");

        RuleFor(v => v.Start)
            .NotEmpty().WithMessage("Start is required.");

        RuleFor(v => v.End)
            .NotEmpty().WithMessage("End is required.")
            .GreaterThanOrEqualTo(v => v.Start).WithMessage("EndDate must be greater than or equal to StartDate.");
    }
}
