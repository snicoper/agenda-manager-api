namespace AgendaManager.WebApi.Controllers.Users.Accounts.Contracts;

public record ResetPasswordRequest(string Token, string NewPassword, string ConfirmNewPassword);
