using AutoMapper;
using Shortha.Application.Dto.Requests.Url;
using Shortha.Application.Dto.Responses.Url;
using Shortha.Application.Exceptions;
using Shortha.Domain;
using Shortha.Domain.Dto;
using Shortha.Domain.Entites;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Application.Services
{
    public class UrlService(IUrlRepository urlRepository, IMapper mapper) : IUrlService
    {
        public async Task<UrlResponse> CreateUrl(UrlCreateRequest urlCreate, string? userId, bool isPremium)
        {
            // Reject premium-only features for free users
            if (!isPremium)
            {
                if (!string.IsNullOrWhiteSpace(urlCreate.CustomHash))
                    throw new NoPermissionException("Custom URL is only available for premium users.");

                if (urlCreate.ExpiresAt.HasValue)
                    throw new NoPermissionException("Expiration date is only available for premium users.");
            }

            var url = new Url
            {
                OriginalUrl = urlCreate.Url,
                UserId = userId,
                ExpiresAt = isPremium ? urlCreate.ExpiresAt : null,
            };

            if (string.IsNullOrWhiteSpace(urlCreate.CustomHash))
            {
                string urlHash = HashGenerator.GenerateHash(url.OriginalUrl + Guid.NewGuid());

                while (await urlRepository.IsHashExists(urlHash))
                {
                    urlHash = HashGenerator.GenerateHash(url.OriginalUrl + Guid.NewGuid());
                }

                url.ShortCode = urlHash;
            }
            else
            {
                bool isExist = await urlRepository.IsHashExists(urlCreate.CustomHash);
                if (isExist)
                    throw new ConflictException("Custom hash already exists. Please choose a different one.");

                url.ShortCode = urlCreate.CustomHash;
            }

            await urlRepository.AddAsync(url);
            await urlRepository.SaveAsync();

            return mapper.Map<UrlResponse>(url);
        }

        public async Task<PaginationResult<UrlResponse>> GetUrlsByUserId(string userId, int page = 1, int pageSize = 10)
        {
            var urls = await urlRepository.GetAsync(filter: u => u.UserId == userId, pageSize: pageSize, pageNumber: page);
            return mapper.Map<PaginationResult<UrlResponse>>(urls);
        }
        public async Task<PublicUrlResponse> OpenUrl(string shortUrl)
        {
            var url = await urlRepository.GetAsync(u => u.ShortCode == shortUrl && u.IsActive);
            if (url is null) throw new NotFoundException("No Url Found");

            if (url.IsExpired)
            {
                url.Deactivate();
                urlRepository.Update(url);
                await urlRepository.SaveAsync();
                throw new NotFoundException("This URL has expired.");
            }



            // Increase the click count
            url.IncrementClick();
            urlRepository.Update(url);
            await urlRepository.SaveAsync();

            //await visitService.Record(track, url.Id);

            return mapper.Map<PublicUrlResponse>(url);
        }

    }

    public interface IUrlService
    {
        Task<UrlResponse> CreateUrl(UrlCreateRequest urlCreate, string? userId, bool isPremium);
        Task<PaginationResult<UrlResponse>> GetUrlsByUserId(string userId, int page = 1, int pageSize = 10);
        Task<PublicUrlResponse> OpenUrl(string shortUrl);
    }
}
