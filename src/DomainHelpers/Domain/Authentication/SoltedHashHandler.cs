using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace DomainHelpers.Domain.Authentication;
/// <summary>
/// Represents salted hash generator.
/// </summary>
public class SaltedPasswordHashHandler {
    /// <summary>
    /// Generate salted hash.
    /// </summary>
    /// <param name="raw"> The source text.</param>
    /// <param name="salt">The salt for adding hash. </param>
    /// <returns></returns>
    public string Generate(string raw, byte[] salt) =>
        Convert.ToBase64String(
          KeyDerivation.Pbkdf2(
            password: raw,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA512,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

    /// <summary>
    /// Generate random salt for the salted hash.
    /// </summary>
    /// <returns>The random salt.</returns>
    public byte[] GenerateRandomSalt() {
        using var gen = RandomNumberGenerator.Create();
        var salt = new byte[128 / 8];
        gen.GetBytes(salt);
        return salt;
    }
}