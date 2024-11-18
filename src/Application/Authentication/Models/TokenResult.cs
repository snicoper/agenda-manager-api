namespace AgendaManager.Application.Authentication.Models;

public record TokenResult(string AccessToken, string RefreshToken, DateTimeOffset Expires);
