using FluentValidation;

namespace AgendaManager.Application.ResourceManagement.ResourceTypes.Commands.UpdateResourceType;

public class UpdateResourceTypeCommandValidator : AbstractValidator<UpdateResourceTypeCommand>
{
    public UpdateResourceTypeCommandValidator()
    {
        RuleFor(r => r.ResourceTypeId)
            .NotEmpty().WithMessage("ResourceTypeId is required.");

        RuleFor(r => r.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");

        RuleFor(r => r.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
    }
}
