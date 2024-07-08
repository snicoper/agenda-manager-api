using AgendaManager.Application.Common.Interfaces.Messaging;

namespace AgendaManager.Application.Authentication.Commands.Register;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string ConfirmPassword) : ICommand<RegisterCommandResponse>
{
}
