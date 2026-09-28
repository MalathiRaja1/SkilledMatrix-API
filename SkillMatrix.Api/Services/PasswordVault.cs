using System.Security.Cryptography;
using System.Text;

namespace SkillMatrix.Api.Services;

// Reversible (AES-GCM) copy of a user's password, so an admin can view it in the
// User Creation grid. Login still uses the salted hash - this copy is only for display.
// The key comes from appsettings ("PasswordViewKey", falling back to Jwt:Key).
public static class PasswordVault
{
    private static byte[] KeyFrom(string secret) => SHA256.HashData(Encoding.UTF8.GetBytes(secret));

    public static string Encrypt(string plain, string secret)
    {
        var nonce = RandomNumberGenerator.GetBytes(12);
        var data = Encoding.UTF8.GetBytes(plain);
        var cipher = new byte[data.Length];
        var tag = new byte[16];

        using var aes = new AesGcm(KeyFrom(secret), 16);
        aes.Encrypt(nonce, data, cipher, tag);

        return Convert.ToBase64String(nonce.Concat(tag).Concat(cipher).ToArray());
    }

    public static string? Decrypt(string? stored, string secret)
    {
        if (string.IsNullOrEmpty(stored)) return null;
        try
        {
            var all = Convert.FromBase64String(stored);
            var nonce = all[..12];
            var tag = all[12..28];
            var cipher = all[28..];
            var plain = new byte[cipher.Length];

            using var aes = new AesGcm(KeyFrom(secret), 16);
            aes.Decrypt(nonce, cipher, tag, plain);
            return Encoding.UTF8.GetString(plain);
        }
        catch
        {
            return null;
        }
    }
}
