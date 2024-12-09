namespace AgendaManager.Infrastructure.Users.Emails.AccountConfirmation;

public record AccountConfirmationViewModel(
    string SiteName,
    string Email,
    string SetPasswordLink,
    int ExpirationDays);
