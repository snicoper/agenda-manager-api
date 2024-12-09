using AgendaManager.Domain.Common.Utils;
using FluentValidation;

namespace AgendaManager.Application.Users.Accounts.Commands.CreateAccount;

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email cannot be empty")
            .EmailAddress().WithMessage("Email is not valid");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name cannot be empty");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name cannot be empty");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password cannot be empty")
            .Matches(DomainRegex.StrongPassword()).WithMessage("The new password is not strong enough.");

        RuleFor(x => x.PasswordConfirmation)
            .NotEmpty().WithMessage("Password confirmation cannot be empty")
            .Equal(x => x.Password).WithMessage("Password and PasswordConfirmation must be equal");

        RuleFor(x => x.Roles)
            .NotEmpty().WithMessage("Roles cannot be empty");
    }
}
