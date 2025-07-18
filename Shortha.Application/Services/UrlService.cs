using AutoMapper;
using Shortha.Application.Dto.Requests.Url;
using Shortha.Domain;
using Shortha.Domain.Entites;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Application.Services
{
    public class UrlService(IUrlRepository urlRepository, IMapper mapper) : IUrlService
    {
        public async Task<Url> CreateUrl(UrlCreateRequest urlCreate, string? customHash = null, string? userId = null)
        {
            var url = new Url()
            {
                OriginalUrl = urlCreate.Url,
            };
            if (customHash == null)
            {
                var urlHash = HashGenerator.GenerateHash(urlCreate.Url);

                // Check if the hash already exists: 1/300,000 Probability of collision, but we can handle it
                while (await urlRepository.IsHashExists(urlHash))
                {
                    urlHash = HashGenerator.GenerateHash(url.OriginalUrl);
                }

                url.ShortCode = urlHash;
            }
            else
            {
                url.ShortCode = customHash;
            }


            await urlRepository.AddAsync(url);
            await urlRepository.SaveAsync();
            return url;
        }
    }
    public interface IUrlService
    {
        Task<Url> CreateUrl(UrlCreateRequest urlCreate, string? customHash = null, string? userId = null);
    }
}
