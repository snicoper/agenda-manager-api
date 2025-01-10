using AgendaManager.Domain.Common.Utils;
using FluentValidation;

namespace AgendaManager.Application.Authentication.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(r => r.Email)
            .NotEmpty()
            .Matches(DomainRegex.ValidEmail()).WithMessage("Invalid email.");

        RuleFor(r => r.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}
