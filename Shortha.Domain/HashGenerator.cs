using System.Security.Cryptography;
using System.Text;

namespace Shortha.Domain;

public static class HashGenerator
{
    private static readonly string Characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    private static readonly int HashLength = 5;

    private static string GetRandomSeed()
    {
        return Guid.NewGuid().ToString();
    }

    public static string GenerateHash(string input)
    {
        var combinedInput = input + GetRandomSeed();

        using (var sha256 = SHA256.Create())
        {
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combinedInput));
            var base64Hash = Convert.ToBase64String(hashBytes);
            var shortHash = new StringBuilder();
            foreach (var character in base64Hash)
            {
                if (Characters.Contains(character))
                {
                    shortHash.Append(character);
                }

                if (shortHash.Length >= HashLength)
                {
                    break;
                }
            }

            return shortHash.ToString();
        }
    }
}