namespace AgendaManager.Domain.Common.Utils;

public static class PasswordGenerator
{
    private const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
    private const string Numbers = "0123456789";
    private const string Specials = "#?!@$%^&*-";

    public static string GeneratePassword(int length = 12)
    {
        if (length < 8)
        {
            throw new ArgumentException("Password length must be at least 8 characters");
        }

        var random = new Random();
        var password = new char[length];

        // Garantizar al menos uno de cada tipo.
        password[0] = Uppercase[random.Next(Uppercase.Length)];
        password[1] = Lowercase[random.Next(Lowercase.Length)];
        password[2] = Numbers[random.Next(Numbers.Length)];
        password[3] = Specials[random.Next(Specials.Length)];

        // Todos los caracteres permitidos para el resto.
        const string allChars = Uppercase + Lowercase + Numbers + Specials;

        // Rellenar el resto de posiciones.
        for (var i = 4; i < length; i++)
        {
            password[i] = allChars[random.Next(allChars.Length)];
        }

        // Mezclar todo para que no siempre siga el mismo patrón.
        return new string(password.OrderBy(_ => random.Next()).ToArray());
    }
}
