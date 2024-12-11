using AgendaManager.Domain.Common.Utils;
using FluentValidation;

namespace AgendaManager.Application.Users.Accounts.Commands.RequestPasswordReset;

public class RequestPasswordResetCommandValidator : AbstractValidator<RequestPasswordResetCommand>
{
    public RequestPasswordResetCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email address is required")
            .Matches(DomainRegex.ValidEmail()).WithMessage("Invalid email address");
    }
}
