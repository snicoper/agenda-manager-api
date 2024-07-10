using AgendaManager.Application.Common.Interfaces.Messaging;
using AgendaManager.Application.Common.Models.Users;

namespace AgendaManager.Application.Authentication.Commands.Login;

public record LoginCommand(string Email, string Password) : ICommand<TokenResponse>;
