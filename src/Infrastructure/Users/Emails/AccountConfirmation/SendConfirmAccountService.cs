using AgendaManager.Application.Users.Interfaces;
using AgendaManager.Domain.Users;
using AgendaManager.Infrastructure.Common.Emails.Constants;
using AgendaManager.Infrastructure.Common.Emails.Interfaces;
using AgendaManager.Infrastructure.Common.Emails.Models;
using AgendaManager.Infrastructure.Common.Options;
using Microsoft.Extensions.Options;

namespace AgendaManager.Infrastructure.Users.Emails.AccountConfirmation;

public class SendConfirmAccountService(
    IEmailService emailService,
    IOptions<ClientApiSettings> apiSettings,
    IOptions<ClientAppSettings> appSettings)
    : ISendConfirmAccountService
{
    public async Task SendAsync(User user, string token, CancellationToken cancellationToken = default)
    {
        var siteName = apiSettings.Value.SiteName;
        var setPasswordLink =
            $"{appSettings.Value.BaseUrl}/accounts/confirm-account?token={Uri.EscapeDataString(token)}";

        // ViewModel.
        var model = new ConfirmAccountViewModel(
            SiteName: siteName,
            Email: user.Email.Value,
            SetPasswordLink: setPasswordLink,
            ExpirationDays: 7);

        // Send email.
        var emailTemplate = new EmailTemplate<ConfirmAccountViewModel>(
            To: [user.Email.Value],
            Subject: $"Bienvenido a {siteName}",
            TemplateName: EmailViewNames.ConfirmAccount,
            Model: model);

        await emailService.SendTemplatedEmailAsync(emailTemplate);
    }
}
