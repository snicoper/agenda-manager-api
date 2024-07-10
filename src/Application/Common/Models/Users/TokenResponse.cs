namespace AgendaManager.Application.Common.Models.Users;

public record TokenResponse(string AccessToken, string RefreshToken, DateTimeOffset Expires);
