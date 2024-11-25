using System.ComponentModel.DataAnnotations;

namespace AgendaManager.Infrastructure.Common.Emails.Options;

public sealed class EmailSettings
{
    public const string SectionName = "EmailSettings";

    [Required]
    public string Host { get; set; } = default!;

    [Required]
    public int Port { get; set; }

    [Required]
    public string Username { get; set; } = default!;

    [Required]
    public string Password { get; set; } = default!;

    [Required]
    public string DefaultFrom { get; set; } = default!;
}
