using FluentValidation;

namespace AgendaManager.Application.Calendars.Commands.UpdateCalendarSettings;

public class UpdateCalendarSettingsCommandValidator : AbstractValidator<UpdateCalendarSettingsCommand>
{
    public UpdateCalendarSettingsCommandValidator()
    {
        RuleFor(v => v.CalendarId)
            .NotEmpty().WithMessage("CalendarId is required.");

        RuleFor(v => v.TimeZone)
            .NotEmpty().WithMessage("TimeZone is required.");

        RuleFor(v => v.AppointmentConfirmationRequirement)
            .IsInEnum().WithMessage("AppointmentConfirmationRequirement is required.");

        RuleFor(v => v.AppointmentOverlapping)
            .IsInEnum().WithMessage("AppointmentOverlapping is required.");

        RuleFor(v => v.HolidayConflict)
            .IsInEnum().WithMessage("HolidayConflict is required.");

        RuleFor(v => v.ResourceScheduleValidation)
            .IsInEnum().WithMessage("ResourceScheduleValidation is required.");
    }
}
