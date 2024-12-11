namespace AgendaManager.Infrastructure.Users.Emails.ResentEmailConfirmation;

public sealed record SendResentEmailConfirmationViewModel(
    string SiteName,
    string Email,
    string ConfirmationLink,
    int ExpirationDays);
