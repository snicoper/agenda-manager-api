using FluentValidation;

namespace AgendaManager.Application.Users.Commands.CreateUser;

public class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator()
    {
        RuleFor(r => r.Email).EmailAddress();

        RuleFor(r => r.Password).NotEmpty().MinimumLength(10);
    }
}
