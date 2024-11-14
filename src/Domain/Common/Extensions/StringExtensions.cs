using System.Globalization;
using System.Text;
using DomainRegex = AgendaManager.Domain.Common.Utils.DomainRegex;

namespace AgendaManager.Domain.Common.Extensions;

public static class StringExtensions
{
    public static string ToLowerFirstLetter(this string value)
    {
        var result = string.IsNullOrWhiteSpace(value) ? value : $"{value[..1].ToLower()}{value[1..]}";

        return result;
    }

    public static string ToUpperFirstLetter(this string value)
    {
        var result = string.IsNullOrWhiteSpace(value) ? value : $"{value[..1].ToUpper()}{value[1..]}";

        return result;
    }

    public static string ToTile(this string value)
    {
        var parts = value.Split()
            .Select(part => part.ToUpperFirstLetter())
            .ToList();

        var result = string.IsNullOrWhiteSpace(value) ? value : string.Join(" ", parts);

        return result;
    }

    public static string Slugify(this string text)
    {
        const string replacement = "-";

        var result = DomainRegex.Slugify()
            .Replace(RemoveDiacritics(text), replacement)
            .Replace("--", "-")
            .Trim('-');

        return result;
    }

    private static string RemoveDiacritics(string text)
    {
        var normalizedString = text.ToLower().Trim().Normalize(NormalizationForm.FormD);

        StringBuilder stringBuilder = new();

        foreach (var character in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(character);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(character);
            }
        }

        var result = stringBuilder.ToString().Normalize(NormalizationForm.FormC);

        return result;
    }
}
