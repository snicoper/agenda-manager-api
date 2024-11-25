using System.ComponentModel.DataAnnotations;

namespace AgendaManager.Infrastructure.Common.Options;

public class WebApiSettings
{
    public const string SectionName = "WebApi";

    [Required]
    public string SiteName { get; set; } = default!;

    [Required]
    public string Scheme { get; set; } = default!;

    [Required]
    public string Host { get; set; } = default!;

    [Required]
    public string ApiSegment { get; set; } = default!;
}
