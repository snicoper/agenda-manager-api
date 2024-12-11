namespace AgendaManager.Infrastructure.Users.Emails.AccountConfirmation;

public record ConfirmAccountViewModel(
    string SiteName,
    string Email,
    string SetPasswordLink,
    int ExpirationDays);
