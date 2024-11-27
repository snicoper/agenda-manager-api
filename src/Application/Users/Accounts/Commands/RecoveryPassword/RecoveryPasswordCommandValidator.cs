using AgendaManager.Domain.Common.Utils;
using FluentValidation;

namespace AgendaManager.Application.Users.Accounts.Commands.RecoveryPassword;

public class RecoveryPasswordCommandValidator : AbstractValidator<RecoveryPasswordCommand>
{
    public RecoveryPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email address is required")
            .Matches(DomainRegex.ValidEmail()).WithMessage("Invalid email address");
    }
}
