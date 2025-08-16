using System.Security.Cryptography;

namespace Shortha.Application
{
    public class Crypto
    {
        public static int GetRandomPrime()
        {
            int[] primes =
            {
                101, 103, 107, 109, 113, 127, 131, 137, 139, 149,
                151, 157, 163, 167, 173, 179, 181, 191, 193, 197, 199
            };

            var rng = RandomNumberGenerator.Create();
            var bytes = new byte[4];
            rng.GetBytes(bytes);
            var index = BitConverter.ToInt32(bytes, 0) % primes.Length;
            if (index < 0) index = -index;

            return primes[index];
        }

        public static string GenerateSHA265FromRaw(string raw)
        {
            using var sha = SHA256.Create();
            var hashBytes = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(raw));
            return Convert.ToBase64String(hashBytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .TrimEnd('=');
        }
    }
}