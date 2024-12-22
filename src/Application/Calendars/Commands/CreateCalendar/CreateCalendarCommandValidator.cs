using FluentValidation;

namespace AgendaManager.Application.Calendars.Commands.CreateCalendar;

public class CreateCalendarCommandValidator : AbstractValidator<CreateCalendarCommand>
{
    public CreateCalendarCommandValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(v => v.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
    }
}
