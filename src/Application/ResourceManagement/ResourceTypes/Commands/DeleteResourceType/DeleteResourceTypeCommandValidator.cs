using FluentValidation;

namespace AgendaManager.Application.ResourceManagement.ResourceTypes.Commands.DeleteResourceType;

public class DeleteResourceTypeCommandValidator : AbstractValidator<DeleteResourceTypeCommand>
{
    public DeleteResourceTypeCommandValidator()
    {
        RuleFor(v => v.ResourceTypeId)
            .NotEmpty().WithMessage("ResourceTypeId is required.");
    }
}
