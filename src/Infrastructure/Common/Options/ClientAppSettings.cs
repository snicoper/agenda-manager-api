using System.ComponentModel.DataAnnotations;

namespace AgendaManager.Infrastructure.Common.Options;

public sealed class ClientAppSettings
{
    public const string SectionName = "ClientApp";

    [Required]
    public string Scheme { get; set; } = default!;

    [Required]
    public string Host { get; set; } = default!;

    public string BaseUrl => $"{Scheme}://{Host}";
}
