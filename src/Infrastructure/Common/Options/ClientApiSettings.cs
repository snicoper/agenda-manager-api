using System.ComponentModel.DataAnnotations;

namespace AgendaManager.Infrastructure.Common.Options;

public sealed class ClientApiSettings
{
    public const string SectionName = "ClientApi";

    [Required]
    public string SiteName { get; set; } = default!;

    [Required]
    public string Scheme { get; set; } = default!;

    [Required]
    public string Host { get; set; } = default!;

    [Required]
    public string ApiSegment { get; set; } = default!;
}
