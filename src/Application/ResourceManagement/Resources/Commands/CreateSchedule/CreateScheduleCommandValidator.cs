using FluentValidation;

namespace AgendaManager.Application.ResourceManagement.Resources.Commands.CreateSchedule;

public class CreateScheduleCommandValidator : AbstractValidator<CreateScheduleCommand>
{
    public CreateScheduleCommandValidator()
    {
        RuleFor(x => x.ResourceId)
            .NotEmpty().WithMessage("Resource Id is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Invalid schedule type.");

        RuleFor(x => x.AvailableDays)
            .IsInEnum().WithMessage("Invalid available days.");

        RuleFor(x => x.Start)
            .NotEmpty().WithMessage("Start date is required.");

        RuleFor(x => x.End)
            .NotEmpty().WithMessage("End date is required.")
            .GreaterThan(x => x.Start).WithMessage("End date must be greater than start date.");
    }
}
