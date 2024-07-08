namespace AgendaManager.WebApi.Contracts.Authentication;

public record LoginRequest(string Email, string Password)
{
}
