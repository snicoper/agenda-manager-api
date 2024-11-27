namespace AgendaManager.WebApi.Controllers.Users.Accounts.Contracts;

public record ConfirmRecoveryPasswordRequest(string Token, string NewPassword, string ConfirmNewPassword);
