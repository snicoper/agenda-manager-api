using AgendaManager.Domain.Common.Utils;
using FluentValidation;

namespace AgendaManager.Application.ResourceManagement.Resources.Commands.CreateResource;

public class CreateResourceCommandValidator : AbstractValidator<CreateResourceCommand>
{
    public CreateResourceCommandValidator()
    {
        RuleFor(x => x.ResourceTypeId)
            .NotEmpty().WithMessage("Resource type is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

        RuleFor(x => x.TextColor)
            .NotEmpty().WithMessage("Text color is required.")
            .Matches(DomainRegex.ValidHexColor()).WithMessage("Text color must be a valid hex color.");

        RuleFor(x => x.BackgroundColor)
            .NotEmpty().WithMessage("Background color is required.")
            .Matches(DomainRegex.ValidHexColor()).WithMessage("Text color must be a valid hex color.");
    }
}
