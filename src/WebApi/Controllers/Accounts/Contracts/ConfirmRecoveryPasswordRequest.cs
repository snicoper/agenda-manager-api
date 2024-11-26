namespace AgendaManager.WebApi.Controllers.Accounts.Contracts;

public record ConfirmRecoveryPasswordRequest(string Token, string NewPassword, string ConfirmNewPassword);
