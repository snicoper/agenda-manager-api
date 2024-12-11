using AgendaManager.Domain.Common.Utils;
using FluentValidation;

namespace AgendaManager.Application.Users.Accounts.Commands.ResentEmailConfirmation;

public class ResentEmailConfirmationValidator : AbstractValidator<ResentEmailConfirmationCommand>
{
    public ResentEmailConfirmationValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email address is required")
            .Matches(DomainRegex.ValidEmail()).WithMessage("Invalid email address");
    }
}
