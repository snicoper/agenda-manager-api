namespace AgendaManager.Infrastructure.Users.Emails.SendEmailConfirmation;

public sealed record SendEmailConfirmationViewModel(
    string SiteName,
    string Email,
    string ConfirmationLink,
    int ExpirationDays);
