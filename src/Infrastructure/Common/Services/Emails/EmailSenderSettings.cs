using System.ComponentModel.DataAnnotations;

namespace AgendaManager.Infrastructure.Common.Services.Emails;

public class EmailSenderSettings
{
    public const string SectionName = "EmailSender";

    [Required]
    public string Host { get; set; } = default!;

    [Required]
    public string DefaultFrom { get; set; } = default!;

    [Required]
    public string Username { get; set; } = default!;

    [Required]
    public string Password { get; set; } = default!;

    [Required]
    public int Port { get; set; }

    [Required]
    public bool UseSsl { get; set; }
}
