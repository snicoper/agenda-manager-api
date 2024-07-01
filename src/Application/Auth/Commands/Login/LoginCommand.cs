using AgendaManager.Application.Common.Interfaces.Messaging;

namespace AgendaManager.Application.Auth.Commands.Login;

public record LoginCommand(string Email, string Password) : ICommand<LoginResponse>;
