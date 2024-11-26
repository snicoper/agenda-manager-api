using AgendaManager.Application.Accounts.Interfaces;
using AgendaManager.Domain.Users;
using AgendaManager.Infrastructure.Common.Emails.Constants;
using AgendaManager.Infrastructure.Common.Emails.Interfaces;
using AgendaManager.Infrastructure.Common.Emails.Models;
using AgendaManager.Infrastructure.Common.Options;
using Microsoft.Extensions.Options;

namespace AgendaManager.Infrastructure.Users.Emails.SendRecoveryPassword;

public class SendRecoveryPasswordService(
    IEmailService emailService,
    IOptions<ClientApiSettings> apiSettings,
    IOptions<ClientAppSettings> appSettings)
    : ISendRecoveryPasswordService
{
    public async Task SendAsync(User user, string token, CancellationToken cancellationToken = default)
    {
        var siteName = apiSettings.Value.SiteName;
        var resetLink =
            $"{appSettings.Value.BaseUrl}/accounts/confirm-recovery-password?token={Uri.EscapeDataString(token)}";

        // ViewModel.
        var model = new SendRecoveryPasswordViewModel(
            SiteName: siteName,
            Email: user.Email.Value,
            ResetLink: resetLink,
            ExpirationHours: 1);

        // Send email.
        var emailTempate = new EmailTemplate<SendRecoveryPasswordViewModel>(
            To: [user.Email.Value],
            Subject: $"Recuperación de contraseña - {siteName}",
            TemplateName: EmailViewNames.RecoveryPassword,
            model);

        await emailService.SendTemplatedEmailAsync(emailTempate);
    }
}
