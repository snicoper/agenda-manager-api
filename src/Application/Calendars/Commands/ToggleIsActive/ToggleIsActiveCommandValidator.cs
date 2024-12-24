using FluentValidation;

namespace AgendaManager.Application.Calendars.Commands.ToggleIsActive;

public class ToggleIsActiveCommandValidator : AbstractValidator<ToggleIsActiveCommand>
{
    public ToggleIsActiveCommandValidator()
    {
        RuleFor(v => v.CalendarId)
            .NotEmpty().WithMessage("CalendarId is required.");
    }
}
