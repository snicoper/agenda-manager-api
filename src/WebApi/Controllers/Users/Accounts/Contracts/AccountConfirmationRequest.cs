namespace AgendaManager.WebApi.Controllers.Users.Accounts.Contracts;

public record AccountConfirmationRequest(string Token, string NewPassword, string ConfirmNewPassword);
