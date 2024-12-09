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

        RuleFor(x => x.Roles)
            .NotEmpty().WithMessage("Roles cannot be empty");
    }
}
