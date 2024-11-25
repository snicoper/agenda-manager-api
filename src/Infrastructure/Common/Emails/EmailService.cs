using System.Net;
using System.Net.Mail;
using AgendaManager.Infrastructure.Common.Emails.Exceptions;
using AgendaManager.Infrastructure.Common.Emails.Interfaces;
using AgendaManager.Infrastructure.Common.Emails.Models;
using AgendaManager.Infrastructure.Common.Emails.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AgendaManager.Infrastructure.Common.Emails;

public class EmailService : IEmailService
{
    private readonly EmailConfiguration _config;
    private readonly ILogger<EmailService> _logger;
    private readonly IHostEnvironment _environment;
    private readonly IRazorViewToStringRenderer _templateRenderer;

    public EmailService(
        IOptions<EmailConfiguration> config,
        ILogger<EmailService> logger,
        IHostEnvironment environment,
        IRazorViewToStringRenderer templateRenderer)
    {
        _config = config.Value;
        _logger = logger;
        _environment = environment;
        _templateRenderer = templateRenderer;
    }

    public async Task SendEmailAsync(EmailMessage message)
    {
        ValidateMessage(message);

        using var mailMessage = CreateMailMessage(message);
        using var client = CreateSmtpClient();

        if (_environment.IsProduction())
        {
            await client.SendMailAsync(mailMessage);
            _logger.LogInformation("Email sent successfully to {Recipients}", string.Join(", ", message.To));
        }
        else
        {
            LogEmailDetails(mailMessage);
        }
    }

    public async Task SendTemplatedEmailAsync<TModel>(EmailTemplate<TModel> template)
        where TModel : class
    {
        var body = await _templateRenderer.RenderViewToStringAsync(
            template.TemplateName,
            template.Model,
            new Dictionary<string, object?>());

        await SendEmailAsync(
            new EmailMessage(
                template.To,
                template.Subject,
                body,
                IsHtml: true));
    }

    private MailMessage CreateMailMessage(EmailMessage message)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress(_config.DefaultFrom),
            Subject = message.Subject,
            Body = message.Body,
            IsBodyHtml = message.IsHtml,
            Priority = message.Priority
        };

        foreach (var recipient in message.To)
        {
            mailMessage.To.Add(recipient);
        }

        return mailMessage;
    }

    private SmtpClient CreateSmtpClient()
    {
        return new SmtpClient
        {
            Host = _config.Host,
            Port = _config.Port,
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_config.Username, _config.Password)
        };
    }

    private void ValidateMessage(EmailMessage message)
    {
        if (!message.To.Any())
        {
            throw new EmailSenderException("At least one recipient is required", nameof(message.To));
        }

        if (string.IsNullOrWhiteSpace(message.Subject))
        {
            throw new EmailSenderException("Subject is required", nameof(message.Subject));
        }

        if (string.IsNullOrWhiteSpace(message.Body))
        {
            throw new EmailSenderException("Body is required", nameof(message.Body));
        }
    }

    private void LogEmailDetails(MailMessage message)
    {
        _logger.LogInformation(
            """
            ========== EMAIL DETAILS ==========
            From: {From}
            To: {To}
            Subject: {Subject}
            Body: {Body}
            ================================
            """,
            message.From,
            string.Join(", ", message.To),
            message.Subject,
            _environment.IsProduction() ? "[BODY HIDDEN IN PRODUCTION]" : message.Body);
    }
}
