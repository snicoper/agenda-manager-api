namespace AgendaManager.WebApi.Controllers.Users.Accounts.Contracts;

public record ConfirmAccountRequest(string Token, string NewPassword, string ConfirmNewPassword);
