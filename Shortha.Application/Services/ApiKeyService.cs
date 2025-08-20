using AutoMapper;
using Shortha.Application.Dto.Responses.Api;
using Shortha.Application.Exceptions;
using Shortha.Domain.Dto;
using Shortha.Domain.Entites;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Application.Services
{
    public interface IApiKeyService
    {
        Task<Api> GenerateApiKeyByUserId(string keyName, string userId, DateTime? expiresAt);
        Task<PaginationResult<ApiKeyResponse>> GetUserKeys(string userId, int page, int pageSize);
        Task<Api> GetApiKeyByKeyAsync(string apiKey);
        Task Update(Api api);
        Task Revoke(string key);
    }

    public class ApiKeyService(IApiRepository repo, IMapper mapper) : IApiKeyService
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
            var apiKey = $"ljeryy.{randomPart}.bygitnasr";
            return apiKey;
        }

        public async Task<Api> GenerateApiKeyByUserId(string keyName, string userId, DateTime? expiresAt)
        {
            // Cehck if the name is already taken
            var existingKey = await repo.GetAsync(a => a.Name == keyName && a.UserId == userId);
            if (existingKey != null)
            {
                throw new ConflictException("An API key" +
                                            " with this name already exists for this user.");
            }

            var apiKey = new Api
            {
                Name = keyName,
                UserId = userId,
                Key = GenerateApiKey(),
                ExpiresAt = expiresAt,
            };

            await repo.AddAsync(apiKey);
            await repo.SaveAsync();

            return apiKey;
        }

        public async Task<PaginationResult<ApiKeyResponse>> GetUserKeys(string userId, int page, int pageSize)
        {
            var keys = await repo.GetAsync(a => a.UserId == userId, page, pageSize, orderBy: a => a.CreatedAt, true);

            return mapper.Map<PaginationResult<ApiKeyResponse>>(keys);
        }

        public async Task<Api> GetApiKeyByKeyAsync(string apiKey)
        {
            var api = await repo.GetAsync(a => a.Key == apiKey, includes: ["User"]);
            if (api == null)
            {
                throw new NotFoundException("API Key not found.");
            }


            return api;
        }

        public async Task Update(Api api)
        {
            repo.Update(api);
            await repo.SaveAsync();
        }

        public async Task Revoke(string keyId)
        {
            var apiKey = await repo.GetAsync(a => a.Id == keyId);
            if (apiKey is not { IsActive: true })
            {
                throw new NotFoundException("API key not found or already revoked.");
            }

            apiKey.Deactivate();
            await Update(apiKey);
        }

        public static bool IsValidApiKey(string candidateApiKey)
        {
            //1. Split it by dots
            var parts = candidateApiKey.Split('.');
            //2. Check if it has exactly 3 parts
            if (parts.Length != 3)
            {
                return false;
            }

            //3. Check if the first part is "ljeryy"
            if (parts[0] != "ljeryy")
            {
                return false;
            }

            //4. Check if the last part is "bygitnasr"
            return parts[2] == "bygitnasr";
        }
    }
}