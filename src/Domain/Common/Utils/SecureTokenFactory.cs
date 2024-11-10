using System.Security.Cryptography;

namespace AgendaManager.Domain.Common.Utils;

public static class SecureTokenFactory
{
    public static string GenerateToken(int tokenLength)
    {
        var randomNumber = new byte[tokenLength / 2];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        // We convert to Base64 and replace problematic characters.
        var token = Convert.ToBase64String(randomNumber)
            .Replace("/", "_")
            .Replace("+", "-")
            .Replace("_", "-")
            .Replace("=", string.Empty);

        if (token.Length > tokenLength)
        {
            token = token[..tokenLength];
        }

        return token;
    }
}
