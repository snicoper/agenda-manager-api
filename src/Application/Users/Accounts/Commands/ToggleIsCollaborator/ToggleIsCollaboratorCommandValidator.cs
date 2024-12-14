using FluentValidation;

namespace AgendaManager.Application.Users.Accounts.Commands.ToggleIsCollaborator;

public class ToggleIsCollaboratorCommandValidator : AbstractValidator<ToggleIsCollaboratorCommand>
{
    public ToggleIsCollaboratorCommandValidator()
    {
        RuleFor(v => v.UserId)
            .NotEmpty().WithMessage("UserId is required.");
    }
}
