using System.Security.Cryptography;
using LeadLane.Domain.Models;

namespace LeadLane.Domain.Services;

public class HashService : IHashService
{
    private const string HashVersion = "v1";
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int Iterations = 100_000;

    /// <summary>
    /// PBKDF2-SHA256. Format: v1.{iterations}.{salt}.{key} (Base64).
    /// </summary>
    public Result<string> HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            HashAlgorithmName.SHA256,
            KeySize);

        var value =
            $"{HashVersion}.{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";

        return Result<string>.Ok(value);
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        var parts = passwordHash.Split('.', 4);
        if (parts.Length != 4 || parts[0] != HashVersion)
        {
            return false;
        }

        if (!int.TryParse(parts[1], out var iterations))
        {
            return false;
        }

        try
        {
            var salt = Convert.FromBase64String(parts[2]);
            var expected = Convert.FromBase64String(parts[3]);
            var actual = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256,
                expected.Length);

            return CryptographicOperations.FixedTimeEquals(actual, expected);
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
