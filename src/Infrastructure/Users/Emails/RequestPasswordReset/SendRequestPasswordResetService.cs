using AgendaManager.Application.Users.Interfaces;
using AgendaManager.Domain.Users;
using AgendaManager.Infrastructure.Common.Emails.Constants;
using AgendaManager.Infrastructure.Common.Emails.Interfaces;
using AgendaManager.Infrastructure.Common.Emails.Models;
using AgendaManager.Infrastructure.Common.Options;
using Microsoft.Extensions.Options;

namespace AgendaManager.Infrastructure.Users.Emails.RequestPasswordReset;

public class SendRequestPasswordResetService(
    IEmailService emailService,
    IOptions<ClientApiSettings> apiSettings,
    IOptions<ClientAppSettings> appSettings)
    : ISendRequestPasswordResetService
{
    public async Task SendAsync(User user, string token, CancellationToken cancellationToken = default)
    {
        var siteName = apiSettings.Value.SiteName;
        var resetLink =
            $"{appSettings.Value.BaseUrl}/accounts/reset-password?token={Uri.EscapeDataString(token)}";

        // ViewModel.
        var model = new SendRequestPasswordResetViewModel(
            SiteName: siteName,
            Email: user.Email.Value,
            ResetLink: resetLink,
            ExpirationHours: 1);

        // Send email.
        var emailTempate = new EmailTemplate<SendRequestPasswordResetViewModel>(
            To: [user.Email.Value],
            Subject: $"Recuperación de contraseña - {siteName}",
            TemplateName: EmailViewNames.RequestPasswordReset,
            model);

        await emailService.SendTemplatedEmailAsync(emailTempate);
    }
}
