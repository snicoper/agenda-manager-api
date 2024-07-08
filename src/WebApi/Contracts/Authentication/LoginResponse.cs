namespace AgendaManager.WebApi.Contracts.Authentication;

public record LoginResponse(string AccessToken, string RefreshToken)
{
}
