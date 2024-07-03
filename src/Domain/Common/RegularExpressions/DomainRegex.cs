using System.Text.RegularExpressions;

namespace AgendaManager.Domain.Common.RegularExpressions;

public abstract partial class DomainRegex
{
    /// <summary>
    /// Esta expresión regular aplicará estas reglas:
    /// • Al menos una letra mayúscula en inglés.
    /// • Al menos una letra minúscula en inglés.
    /// • Al menos un dígito.
    /// • Al menos un carácter especial.
    /// • Mínimo de 8 caracteres de longitud.
    /// </summary>
    [GeneratedRegex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", RegexOptions.Compiled)]
    public static partial Regex StrongPasswordRegex();

    /// <summary>
    /// Caracteres válidos para un slug.
    /// </summary>
    [GeneratedRegex(@"[^a-zA-Z0-9\-]", RegexOptions.Compiled)]
    public static partial Regex SlugifyRegex();

    /// <summary>
    /// Validar correo electrónico.
    /// </summary>
    [GeneratedRegex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", RegexOptions.Compiled)]
    public static partial Regex EmailRegex();
}
