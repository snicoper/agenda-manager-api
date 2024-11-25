using System.Net.Mail;

namespace AgendaManager.Infrastructure.Common.Emails.Models;

public record EmailMessage(
    IEnumerable<string> To,
    string Subject,
    string Body,
    bool IsHtml = false,
    MailPriority Priority = MailPriority.Normal);
