using System.ComponentModel.DataAnnotations;

namespace AgendaManager.Infrastructure.Common.Options;

public class WebAppSettings
{
    public const string SectionName = "WebApp";

    [Required]
    public string Scheme { get; set; } = default!;

    [Required]
    public string Host { get; set; } = default!;
}
