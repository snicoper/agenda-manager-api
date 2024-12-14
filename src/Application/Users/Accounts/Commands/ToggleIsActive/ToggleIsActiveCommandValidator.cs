using FluentValidation;

namespace AgendaManager.Application.Users.Accounts.Commands.ToggleIsActive;

public class ToggleIsActiveCommandValidator : AbstractValidator<ToggleIsActiveCommand>
{
    public ToggleIsActiveCommandValidator()
    {
        RuleFor(v => v.UserId)
            .NotEmpty().WithMessage("UserId is required.");
    }
}
