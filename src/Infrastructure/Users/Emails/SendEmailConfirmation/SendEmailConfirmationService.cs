using AgendaManager.Application.Accounts.Interfaces;
using AgendaManager.Domain.Users;
using AgendaManager.Infrastructure.Common.Emails.Constants;
using AgendaManager.Infrastructure.Common.Emails.Interfaces;
using AgendaManager.Infrastructure.Common.Emails.Models;
using AgendaManager.Infrastructure.Common.Options;
using Microsoft.Extensions.Options;

namespace AgendaManager.Infrastructure.Users.Emails.SendEmailConfirmation;

public sealed class SendEmailConfirmationService(
    IEmailService emailService,
    IOptions<ClientApiSettings> apiSettings,
    IOptions<ClientAppSettings> appSettings)
    : ISendEmailConfirmationService
{
    public async Task SendAsync(User user, string token, CancellationToken cancellationToken = default)
    {
        var siteName = apiSettings.Value.SiteName;
        var resetLink =
            $"{appSettings.Value.BaseUrl}/accounts/email-code-verify?token={Uri.EscapeDataString(token)}";

        // ViewModel.
        var model = new SendEmailConfirmationViewModel(
            SiteName: siteName,
            Email: user.Email.Value,
            ConfirmationLink: resetLink,
            ExpirationDays: 7);

        // Send email.
        var emailTempate = new EmailTemplate<SendEmailConfirmationViewModel>(
            To: [user.Email.Value],
            Subject: $"Confirmación de correo electrónico - {siteName}",
            TemplateName: EmailViewNames.SendEmailConfirmation,
            model);

        await emailService.SendTemplatedEmailAsync(emailTempate);
    }
}
