namespace AgendaManager.Application.Auth.Commands.Login;

public sealed record LoginResponse(string AccessToken, string RefreshToken);
