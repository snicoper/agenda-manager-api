using AgendaManager.Infrastructure.Common.Emails.Models;

namespace AgendaManager.Infrastructure.Common.Emails.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(EmailMessage message);

    Task SendTemplatedEmailAsync<TModel>(EmailTemplate<TModel> template)
        where TModel : class;
}
