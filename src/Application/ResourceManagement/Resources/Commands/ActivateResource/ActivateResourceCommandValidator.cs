using FluentValidation;

namespace AgendaManager.Application.ResourceManagement.Resources.Commands.ActivateResource;

public class ActivateResourceCommandValidator : AbstractValidator<ActivateResourceCommand>
{
    public ActivateResourceCommandValidator()
    {
        RuleFor(v => v.ResourceId)
            .NotEmpty().WithMessage("ResourceId is required.");
    }
}
