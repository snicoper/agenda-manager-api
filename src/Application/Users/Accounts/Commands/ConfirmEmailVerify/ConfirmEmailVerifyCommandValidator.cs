using FluentValidation;

namespace AgendaManager.Application.Users.Accounts.Commands.ConfirmEmailVerify;

public class ConfirmEmailVerifyCommandValidator : AbstractValidator<ConfirmEmailVerifyCommand>
{
    public ConfirmEmailVerifyCommandValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Token is required");
    }
}
