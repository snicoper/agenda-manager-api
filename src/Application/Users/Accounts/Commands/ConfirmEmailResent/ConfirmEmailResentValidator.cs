using AgendaManager.Domain.Common.Utils;
using FluentValidation;

namespace AgendaManager.Application.Users.Accounts.Commands.ConfirmEmailResent;

public class ConfirmEmailResentValidator : AbstractValidator<ConfirmEmailResentCommand>
{
    public ConfirmEmailResentValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email address is required")
            .Matches(DomainRegex.ValidEmail()).WithMessage("Invalid email address");
    }
}
