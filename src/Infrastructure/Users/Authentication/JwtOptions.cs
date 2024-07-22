using System.ComponentModel.DataAnnotations;

namespace AgendaManager.Infrastructure.Users.Authentication;

public class JwtOptions
{
    public const string SectionName = "Jwt";

    [Range(10, int.MaxValue)]
    public int AccessTokenLifeTimeMinutes { get; set; }

    [Range(1, int.MaxValue)]
    public int RefreshTokenLifeTimeDays { get; set; }

    [Required]
    public string Issuer { get; set; } = default!;

    [Required]
    public string Audience { get; set; } = default!;

    [MinLength(32)]
    public string Key { get; set; } = default!;
}
