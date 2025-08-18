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
            var keys = await repo.GetAsync(a => a.UserId == userId, page, pageSize);

            return mapper.Map<PaginationResult<ApiKeyResponse>>(keys);
        }
    }
}