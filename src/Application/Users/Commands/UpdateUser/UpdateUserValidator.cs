using FluentValidation;

namespace AgendaManager.Application.Users.Commands.UpdateUser;

public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidator()
    {
        RuleFor(r => r.Id).NotEmpty();

        RuleFor(r => r.Email).EmailAddress();
    }
}
