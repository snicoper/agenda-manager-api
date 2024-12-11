using AgendaManager.Application.Users.Interfaces;
using AgendaManager.Domain.Users;
using AgendaManager.Infrastructure.Common.Emails.Constants;
using AgendaManager.Infrastructure.Common.Emails.Interfaces;
using AgendaManager.Infrastructure.Common.Emails.Models;
using AgendaManager.Infrastructure.Common.Options;
using Microsoft.Extensions.Options;

namespace AgendaManager.Infrastructure.Users.Emails.ResentEmailConfirmation;

public sealed class SendResentEmailConfirmationService(
    IEmailService emailService,
    IOptions<ClientApiSettings> apiSettings,
    IOptions<ClientAppSettings> appSettings)
    : ISendResentEmailConfirmationService
{
    public async Task SendAsync(User user, string token, CancellationToken cancellationToken = default)
    {
        var siteName = apiSettings.Value.SiteName;
        var resetLink =
            $"{appSettings.Value.BaseUrl}/accounts/verify-email?token={Uri.EscapeDataString(token)}";

        // ViewModel.
        var model = new SendResentEmailConfirmationViewModel(
            SiteName: siteName,
            Email: user.Email.Value,
            ConfirmationLink: resetLink,
            ExpirationDays: 7);

        // Send email.
        var emailTempate = new EmailTemplate<SendResentEmailConfirmationViewModel>(
            To: [user.Email.Value],
            Subject: $"Confirmación de correo electrónico - {siteName}",
            TemplateName: EmailViewNames.ResentEmailConfirmation,
            model);

        await emailService.SendTemplatedEmailAsync(emailTempate);
    }
}
