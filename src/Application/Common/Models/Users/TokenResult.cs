namespace AgendaManager.Application.Common.Models.Users;

public record TokenResult(string AccessToken, string RefreshToken, DateTimeOffset Expires);
