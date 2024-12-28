using FluentValidation;

namespace AgendaManager.Application.ResourceManagement.ResourceTypes.Commands.CreateResourceType;

public class CreateResourceTypeCommandValidator : AbstractValidator<CreateResourceTypeCommand>
{
    public CreateResourceTypeCommandValidator()
    {
        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");

        RuleFor(v => v.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

        RuleFor(v => v.Category)
            .IsInEnum().WithMessage("Invalid category.");
    }
}
