using System.Text.RegularExpressions;

namespace AgendaManager.Domain.Common.Constants;

public static partial class DomainRegex
{
    /// <summary>
    /// Email valido.
    /// </summary>
    [GeneratedRegex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", RegexOptions.Compiled)]
    public static partial Regex ValidEmail();

    /// <summary>
    /// Caracteres validos para el slug.
    /// </summary>
    [GeneratedRegex(@"[^a-zA-Z0-9\-]", RegexOptions.Compiled)]
    public static partial Regex Slugify();
}
