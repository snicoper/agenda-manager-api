namespace AgendaManager.Infrastructure.Users.Emails.RequestPasswordReset;

public record SendRequestPasswordResetViewModel(string SiteName, string Email, string ResetLink, int ExpirationHours);
