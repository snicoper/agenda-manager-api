using AgendaManager.Domain.Common.Utils;
using FluentValidation;

namespace AgendaManager.Application.Accounts.Commands.ResendEmailCode;

public class ResendEmailCodeValidator : AbstractValidator<ResendEmailCodeCommand>
{
    public ResendEmailCodeValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email address is required")
            .Matches(DomainRegex.ValidEmail()).WithMessage("Invalid email address");
    }
}
