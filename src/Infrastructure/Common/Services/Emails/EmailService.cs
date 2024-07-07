using System.Net;
using System.Net.Mail;
using System.Text;
using AgendaManager.Application.Common.Interfaces.Emails;
using AgendaManager.Application.Common.Interfaces.Views;
using AgendaManager.Infrastructure.Common.Exceptions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AgendaManager.Infrastructure.Common.Services.Emails;

public class EmailService : IEmailService
{
    private readonly EmailSenderSettings _emailSenderSettings;
    private readonly IHostEnvironment _environment;
    private readonly ILogger<EmailService> _logger;
    private readonly IRazorViewToStringRenderer _razorViewToStringRenderer;

    public EmailService(
        IOptions<EmailSenderSettings> options,
        ILogger<EmailService> logger,
        IHostEnvironment environment,
        IRazorViewToStringRenderer razorViewToStringRenderer)
    {
        _logger = logger;
        _environment = environment;
        _razorViewToStringRenderer = razorViewToStringRenderer;
        _emailSenderSettings = options.Value;

        MailPriority = MailPriority.High;
        From = _emailSenderSettings.DefaultFrom;
        IsBodyHtml = false;
    }

    public MailPriority MailPriority { get; set; }

    public string From { get; set; }

    public ICollection<string> To { get; set; } = [];

    public string? Subject { get; set; }

    public string? Body { get; set; }

    public bool IsBodyHtml { get; set; }

    public async Task SendMailWithViewAsync<TModel>(string viewName, TModel model)
        where TModel : class
    {
        IsBodyHtml = true;
        Body = await _razorViewToStringRenderer.RenderViewToStringAsync(viewName, model, []);

        Send();
    }

    public void SendMail()
    {
        Send();
    }

    private void Send()
    {
        using var mailMessage = new MailMessage();
        mailMessage.From = new MailAddress(From);
        mailMessage.To.Add(string.Join(",", To));
        mailMessage.Subject = Subject;
        mailMessage.Body = Body;
        mailMessage.IsBodyHtml = IsBodyHtml;
        mailMessage.Priority = MailPriority;

        using var client = new SmtpClient();
        client.Host = _emailSenderSettings.Host;
        client.Port = _emailSenderSettings.Port;
        client.Credentials = new NetworkCredential(_emailSenderSettings.Username, _emailSenderSettings.Password);
        client.UseDefaultCredentials = false;
        client.EnableSsl = true;

        ValidateEmail();

        // Solo en Production se envían los mensajes por SMTP.
        if (!_environment.IsProduction())
        {
            LoggerMessage();
            return;
        }

        client.Send(mailMessage);
    }

    private void ValidateEmail()
    {
        if (To.Count == 0)
        {
            throw new EmailSenderException("The value cannot be an empty string. (Parameter 'To')");
        }

        if (string.IsNullOrEmpty(Subject))
        {
            throw new EmailSenderException("The value cannot be an empty string. (Parameter 'Subject')");
        }

        if (string.IsNullOrEmpty(Body))
        {
            throw new EmailSenderException("The value cannot be an empty string. (Parameter 'Body')");
        }
    }

    private void LoggerMessage()
    {
        var to = string.Join(", ", To);
        var body = !_environment.IsProduction() ? Body : "Body here.....";

        var stringBuilder = new StringBuilder();
        stringBuilder.Append("=========================================================\n");
        stringBuilder.Append($"From: {From}\n");
        stringBuilder.Append($"To: {to}\n");
        stringBuilder.Append($"Subject: {Subject}\n");
        stringBuilder.Append("=========================================================\n");
        stringBuilder.Append($"Body: {body}\n");
        stringBuilder.Append("=========================================================\n");

        _logger.LogDebug("{LogEmail}", stringBuilder);
    }
}
