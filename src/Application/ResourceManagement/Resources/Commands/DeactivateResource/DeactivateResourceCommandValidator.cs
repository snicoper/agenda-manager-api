using FluentValidation;

namespace AgendaManager.Application.ResourceManagement.Resources.Commands.DeactivateResource;

public class DeactivateResourceCommandValidator : AbstractValidator<DeactivateResourceCommand>
{
    public DeactivateResourceCommandValidator()
    {
        RuleFor(v => v.ResourceId)
            .NotEmpty().WithMessage("ResourceId is required.");

        RuleFor(v => v.DeactivationReason)
            .NotEmpty().WithMessage("DeactivationReason is required.")
            .MaximumLength(256).WithMessage("DeactivationReason must not exceed 256 characters.");
    }
}
