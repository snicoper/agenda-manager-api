namespace AgendaManager.WebApi.Options;

public sealed class CorsSettings
{
    public const string SectionName = "Cors";

    public string DefaultPolicyName { get; init; } = null!;

    public string[] AllowedDomains { get; init; } = [];

    public string[] AllowedMethods { get; init; } = [];

    public string[] AllowedHeaders { get; init; } = [];

    public string[] ExposedHeaders { get; init; } = [];
}
