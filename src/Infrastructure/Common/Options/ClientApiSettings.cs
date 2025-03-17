using System.ComponentModel.DataAnnotations;

namespace AgendaManager.Infrastructure.Common.Options;

public sealed class ClientApiSettings
{
    public const string SectionName = "ClientApi";

    [Required]
    public string SiteName { get; set; } = null!;

    [Required]
    public string Scheme { get; set; } = null!;

    [Required]
    public string Host { get; set; } = null!;

    [Required]
    public string ApiSegment { get; set; } = null!;
}
