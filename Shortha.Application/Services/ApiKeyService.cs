using Shortha.Domain.Entites;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Application.Services
{
    public interface IApiKeyService
    {
        Task GenerateApiKeyByUserId(string keyName, string userId, DateTime? expiresAt);
    }

    public class ApiKeyService(IApiRepository repo) : IApiKeyService
    {
        private string GenerateApiKey()
        {
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();

            // random part (main body)
            var keyBytes = new byte[24]; // 192-bit
            rng.GetBytes(keyBytes);
            var randomPart = Convert.ToBase64String(keyBytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .TrimEnd('=');


            // final format
            var apiKey = $"ljeryy_{randomPart}_.bygitnasr";
            return apiKey;
        }

        public async Task GenerateApiKeyByUserId(string keyName, string userId, DateTime? expiresAt)
        {
            var apiKey = new Api
            {
                Name = keyName,
                UserId = userId,
                Key = GenerateApiKey(),
                ExpiresAt = expiresAt,
            };

            await repo.AddAsync(apiKey);
            await repo.SaveAsync();
        }
    }
}