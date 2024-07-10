namespace AgendaManager.WebApi.Controllers.Authentication.Contracts;

public record RegisterRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string ConfirmPassword);
