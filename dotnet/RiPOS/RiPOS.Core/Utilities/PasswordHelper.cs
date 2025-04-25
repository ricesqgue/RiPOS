using System.Security.Cryptography;

namespace RiPOS.Core.Utilities;

public static class PasswordHelper
{
    private static byte[] GenerateSalt()
    {
        byte[] salt = new byte[16];
        RandomNumberGenerator.Create().GetBytes(salt);
        return salt;
    }

    public static byte[] GenerateHash(string password, byte[] salt)
    {
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 1000, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(20);
        return hash;
    }

    public static string GeneratePasswordHashString(string password)
    {
        var salt = GenerateSalt();

        var hash = GenerateHash(password, salt);

        byte[] hashBytes = new byte[36];

        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 20);

        return Convert.ToBase64String(hashBytes);
    }

    public static byte[] GetSaltFromHashBytes(byte[] hashBytes)
    {
        byte[] salt = new byte[16];
        Array.Copy(hashBytes, 0, salt, 0, 16);

        return salt;
    }

    public static bool IsCorrectPassword(byte[] hashBytes1, byte[] hashBytes2)
    {
        for (int i = 0; i < 20; i++)
        {
            if (hashBytes1[i + 16] != hashBytes2[i])
            {
                return false;
            }
        }
        return true;
    }
}