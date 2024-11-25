using AgendaManager.Application.Accounts.Interfaces;
using AgendaManager.Domain.Users;
using AgendaManager.Infrastructure.Common.Emails.Constants;
using AgendaManager.Infrastructure.Common.Emails.Interfaces;
using AgendaManager.Infrastructure.Common.Emails.Models;
using AgendaManager.Infrastructure.Common.Options;
using Microsoft.Extensions.Options;

namespace AgendaManager.Infrastructure.Users.Emails.SendRecoveryPassword;

public class SendRecoveryPasswordService(IEmailService emailService, IOptions<WebApiSettings> apiSettings)
    : ISendRecoveryPasswordService
{
    public async Task SendAsync(User user, string token, CancellationToken cancellationToken = default)
    {
        var siteName = apiSettings.Value.SiteName;

        // ViewModel.
        var model = new SendRecoveryPasswordViewModel(siteName, user.Email.Value, token);

        // Send email.
        var emailTempate = new EmailTemplate<SendRecoveryPasswordViewModel>(
            To: [user.Email.Value],
            Subject: "Recovery Password",
            TemplateName: EmailViewNames.RecoveryPassword,
            model);

        await emailService.SendTemplatedEmailAsync(emailTempate);
    }
}
