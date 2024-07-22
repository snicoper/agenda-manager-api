namespace AgendaManager.WebApi.Controllers.Users.Contracts;

public record RegisterRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string ConfirmPassword);
