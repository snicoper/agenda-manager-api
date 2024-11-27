using AgendaManager.Application.Users.Interfaces;
using AgendaManager.Domain.Users;
using AgendaManager.Infrastructure.Common.Emails.Constants;
using AgendaManager.Infrastructure.Common.Emails.Interfaces;
using AgendaManager.Infrastructure.Common.Emails.Models;
using AgendaManager.Infrastructure.Common.Options;
using Microsoft.Extensions.Options;

namespace AgendaManager.Infrastructure.Users.Emails.ConfirmEmailResent;

public sealed class SendConfirmEmailResentService(
    IEmailService emailService,
    IOptions<ClientApiSettings> apiSettings,
    IOptions<ClientAppSettings> appSettings)
    : ISendConfirmEmailResentService
{
    public async Task SendAsync(User user, string token, CancellationToken cancellationToken = default)
    {
        var siteName = apiSettings.Value.SiteName;
        var resetLink =
            $"{appSettings.Value.BaseUrl}/accounts/confirm-email-verify?token={Uri.EscapeDataString(token)}";

        // ViewModel.
        var model = new SendConfirmEmailResentViewModel(
            SiteName: siteName,
            Email: user.Email.Value,
            ConfirmationLink: resetLink,
            ExpirationDays: 7);

        // Send email.
        var emailTempate = new EmailTemplate<SendConfirmEmailResentViewModel>(
            To: [user.Email.Value],
            Subject: $"Confirmación de correo electrónico - {siteName}",
            TemplateName: EmailViewNames.ConfirmEmailResent,
            model);

        await emailService.SendTemplatedEmailAsync(emailTempate);
    }
}
