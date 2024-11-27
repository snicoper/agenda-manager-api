namespace AgendaManager.Infrastructure.Users.Emails.ConfirmEmailResent;

public sealed record SendConfirmEmailResentViewModel(
    string SiteName,
    string Email,
    string ConfirmationLink,
    int ExpirationDays);
