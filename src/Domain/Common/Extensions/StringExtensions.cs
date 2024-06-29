using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace AgendaManager.Domain.Common.Extensions;

public static class StringExtensions
{
    public static string ToLowerFirstLetter(this string value)
    {
        var result = string.IsNullOrEmpty(value) ? value : $"{value[..1].ToLower()}{value[1..]}";

        return result;
    }

    public static string ToUpperFirstLetter(this string value)
    {
        var result = string.IsNullOrEmpty(value) ? value : $"{value[..1].ToUpper()}{value[1..]}";

        return result;
    }

    public static string ToTile(this string value)
    {
        var parts = value.Split()
            .Select(part => part.ToUpperFirstLetter())
            .ToList();

        var result = string.IsNullOrEmpty(value) ? value : string.Join(" ", parts);

        return result;
    }

    public static string Slugify(this string text)
    {
        const string pattern = @"[^a-zA-Z0-9\-]";
        const string replacement = "-";
        var regex = new Regex(pattern);
        var result = regex
            .Replace(RemoveDiacritics(text), replacement)
            .Replace("--", "-")
            .Trim('-');

        return result;
    }

    private static string RemoveDiacritics(string text)
    {
        var normalizedString = text.ToLower().Trim().Normalize(NormalizationForm.FormD);

        var stringBuilder = new StringBuilder();
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
