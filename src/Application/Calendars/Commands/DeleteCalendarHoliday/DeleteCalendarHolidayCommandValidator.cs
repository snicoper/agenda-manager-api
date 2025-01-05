using FluentValidation;

namespace AgendaManager.Application.Calendars.Commands.DeleteCalendarHoliday;

public class DeleteCalendarHolidayCommandValidator : AbstractValidator<DeleteCalendarHolidayCommand>
{
    public DeleteCalendarHolidayCommandValidator()
    {
        RuleFor(v => v.CalendarHolidayId)
            .NotEmpty().WithMessage("CalendarHolidayId is required.");
    }
}
