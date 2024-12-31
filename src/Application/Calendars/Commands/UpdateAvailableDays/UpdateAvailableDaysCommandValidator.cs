using FluentValidation;

namespace AgendaManager.Application.Calendars.Commands.UpdateAvailableDays;

public class UpdateAvailableDaysCommandValidator : AbstractValidator<UpdateAvailableDaysCommand>
{
    public UpdateAvailableDaysCommandValidator()
    {
        RuleFor(v => v.CalendarId)
            .NotEmpty().WithMessage("CalendarId is required.");

        RuleFor(v => v.AvailableDays)
            .IsInEnum().WithMessage("AvailableDays is not valid.");
    }
}
