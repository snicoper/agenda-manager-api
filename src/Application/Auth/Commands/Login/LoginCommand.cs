namespace AgendaManager.Application.Auth.Commands.Login;

public sealed record class LoginCommand(string Email, string Password);
