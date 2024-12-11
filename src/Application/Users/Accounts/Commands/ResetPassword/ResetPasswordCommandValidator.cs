using AgendaManager.Domain.Common.Utils;
using FluentValidation;

namespace AgendaManager.Application.Users.Accounts.Commands.ResetPassword;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("The token is invalid or expired.");

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .Matches(DomainRegex.StrongPassword()).WithMessage("The new password is not strong enough.");

        RuleFor(x => x.ConfirmNewPassword)
            .NotEmpty()
            .Equal(x => x.NewPassword).WithMessage("The new password and confirm new password must be equal.");
    }
}
