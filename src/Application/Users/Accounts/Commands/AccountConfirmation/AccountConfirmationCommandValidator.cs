using FluentValidation;

namespace AgendaManager.Application.Users.Accounts.Commands.AccountConfirmation;

public class AccountConfirmationCommandValidator : AbstractValidator<AccountConfirmationCommand>
{
    public AccountConfirmationCommandValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Token is required.");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("New password is required.");

        RuleFor(x => x.ConfirmNewPassword)
            .NotEmpty().WithMessage("Confirm new password is required.")
            .Equal(x => x.NewPassword).WithMessage("New password and confirm new password must be the same.");
    }
}
