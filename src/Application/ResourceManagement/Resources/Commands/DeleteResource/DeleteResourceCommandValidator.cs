using FluentValidation;

namespace AgendaManager.Application.ResourceManagement.Resources.Commands.DeleteResource;

public class DeleteResourceCommandValidator : AbstractValidator<DeleteResourceCommand>
{
    public DeleteResourceCommandValidator()
    {
        RuleFor(v => v.ResourceId)
            .NotEmpty().WithMessage("ResourceId is required.");
    }
}
