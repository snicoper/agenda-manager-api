using AgendaManager.Domain.Common.Utils;
using FluentValidation;

namespace AgendaManager.Application.ResourceManagement.Resources.Commands.UpdateResource;

public class UpdateResourceCommandValidator : AbstractValidator<UpdateResourceCommand>
{
    public UpdateResourceCommandValidator()
    {
        RuleFor(v => v.ResourceId)
            .NotEmpty();

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(v => v.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

        RuleFor(v => v.TextColor)
            .MaximumLength(7)
            .Matches(DomainRegex.ValidHexColor()).WithMessage("Invalid hex color.");

        RuleFor(v => v.BackgroundColor)
            .MaximumLength(7)
            .Matches(DomainRegex.ValidHexColor()).WithMessage("Invalid hex color.");
    }
}
