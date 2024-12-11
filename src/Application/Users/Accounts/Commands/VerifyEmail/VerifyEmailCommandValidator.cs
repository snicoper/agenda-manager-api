using FluentValidation;

namespace AgendaManager.Application.Users.Accounts.Commands.VerifyEmail;

public class VerifyEmailCommandValidator : AbstractValidator<VerifyEmailCommand>
{
    public VerifyEmailCommandValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Token is required");
    }
}
