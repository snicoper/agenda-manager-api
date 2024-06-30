using System.ComponentModel.DataAnnotations;

namespace AgendaManager.Application.Common.Settings;

public class JwtSettings
{
    public const string SectionName = "Jwt";

    [Range(10, int.MaxValue)]
    public int AccessTokenLifeTimeMinutes { get; set; }

    [Range(1, int.MaxValue)]
    public int RefreshTokenLifeTimeDays { get; set; }

    [Required]
    public string? Issuer { get; set; }

    [Required]
    public string? Audience { get; set; }

    [MinLength(32)]
    public string Key { get; set; } = default!;
}
