using System.Text.RegularExpressions;

namespace AgendaManager.Domain.Common.Utils;

public static partial class DomainRegex
{
    /// <summary>
    /// Verificación de email.
    /// </summary>
    [GeneratedRegex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", RegexOptions.Compiled)]
    public static partial Regex ValidEmail();

    /// <summary>
    /// Caracteres validos para el slug.
    /// </summary>
    [GeneratedRegex(@"[^a-zA-Z0-9\-]", RegexOptions.Compiled)]
    public static partial Regex Slugify();

    /// <summary>
    /// Verificación de password de seguridad.
    /// • Al menos una letra mayúscula.
    /// • Al menos una letra minúscula.
    /// • Al menos un dígito.
    /// • Al menos un carácter especial.
    /// • Mínimo 8 de longitud.
    /// </summary>
    [GeneratedRegex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", RegexOptions.Compiled)]
    public static partial Regex StrongPassword();

    /// <summary>
    /// Verificación de color hexadecimal.
    /// </summary>
    [GeneratedRegex("^#[0-9A-Fa-f]{6}$", RegexOptions.Compiled)]
    public static partial Regex ValidHexColor();
}
