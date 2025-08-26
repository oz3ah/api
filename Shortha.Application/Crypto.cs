using System.Security.Cryptography;
using System.Text;

namespace Shortha.Application
{
    public class Crypto
    {
        public static async Task<string> GenerateBodyHashAsync(string body)
        {
            using var sha256 = SHA256.Create();
            var data = Encoding.UTF8.GetBytes(body);

            var hashBytes = await Task.Run(() => sha256.ComputeHash(data));

            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }

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

        public static string GenerateSecretKey(int length = 32)
        {
            const string chars = "123456780-=+!@#$%^&*()_QWERTYUIOP{}ASDFGHJKL:ZXCVBNM<>?";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string GeneratePairCode()
        {
            const int length = 6;
            Span<byte> bytes = stackalloc byte[length];
            RandomNumberGenerator.Fill(bytes);
            var chars = new char[length];
            for (var i = 0; i < length; i++)
                chars[i] = (char)('0' + (bytes[i] % 10)); // allows leading zeros
            return new string(chars);
        }

        public static string GenerateApiToken(int numBytes = 32)
        {
            var bytes = new byte[numBytes];
            RandomNumberGenerator.Fill(bytes);
            return Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }
    }
}