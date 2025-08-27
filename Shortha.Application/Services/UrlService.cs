using AutoMapper;
using Microsoft.AspNetCore.Http;
using Shortha.Application.Dto.Requests.Url;
using Shortha.Application.Dto.Responses.Url;
using Shortha.Application.Exceptions;
using Shortha.Application.Interfaces;
using Shortha.Application.Interfaces.Services;
using Shortha.Domain;
using Shortha.Domain.Dto;
using Shortha.Domain.Entites;
using Shortha.Domain.Enums;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Application.Services
{
    public class UrlService(
        IUrlRepository repo,
        IMapper mapper,
        IBackgroundJobService background
    ) : IUrlService
    {
        private UrlCreationSource MapSource(string cadidateSource)
        {
            return cadidateSource switch
            {
                "AuthenticationTypes.Federation" => UrlCreationSource.ProfilePage,
                "ApiKey" => UrlCreationSource.ApiKey,
                "SignedRequest" => UrlCreationSource.ConnectedApp,
                _ => UrlCreationSource.HomePage
            };
        }

        public async Task<UrlResponse> CreateUrl(UrlCreateRequest urlCreate, string? userId,
            HashSet<string> permissions, string source)
        {
            if (!permissions.Contains("create:custom") && !string.IsNullOrWhiteSpace(urlCreate.CustomHash))
                throw new NoPermissionException("Custom URL is only available for premium users.");

            if (!permissions.Contains("create:expire") && urlCreate.ExpiresAt.HasValue)
                throw new NoPermissionException("Expiration date is only available for premium users.");

            var url = new Url
            {
                OriginalUrl = urlCreate.Url,
                UserId = userId,
                ExpiresAt = permissions.Contains("create:expire") ? urlCreate.ExpiresAt : null,
                CreationSource = MapSource(source)
            };

            if (string.IsNullOrWhiteSpace(urlCreate.CustomHash))
            {
                string urlHash = HashGenerator.GenerateHash(url.OriginalUrl + Guid.NewGuid());

                while (await repo.IsHashExists(urlHash))
                {
                    urlHash = HashGenerator.GenerateHash(url.OriginalUrl + Guid.NewGuid());
                }

                url.ShortCode = urlHash;
            }
            else
            {
                bool isExist = await repo.IsHashExists(urlCreate.CustomHash);
                if (isExist)
                    throw new ConflictException("Custom hash already exists. Please choose a different one.");

                url.ShortCode = urlCreate.CustomHash;
            }

            await repo.AddAsync(url);
            await repo.SaveAsync();

            return mapper.Map<UrlResponse>(url);
        }

        public async Task<PaginationResult<UrlResponse>> GetUrlsByUserId(string userId, int page = 1, int pageSize = 10)
        {
            var urls = await repo.GetAsync(filter: u => u.UserId == userId, pageSize: pageSize, pageNumber: page,
                orderBy: u => u.CreatedAt, descending: true);
            return mapper.Map<PaginationResult<UrlResponse>>(urls);
        }

        public async Task<PublicUrlResponse> OpenUrl(string shortUrl, RequestInfo request)
        {
            var url = await repo.GetAsync(u => u.ShortCode == shortUrl && u.IsActive);
            if (url is null) throw new NotFoundException("No Url Found");

            if (url.IsExpired)
            {
                throw new UrlAccessException("This URL has expired.");
            }


            url.IncrementClick();
            repo.Update(url);
            await repo.SaveAsync();

            background.Enqueue<IVisitService>((x) => x.Record(request, url.Id));

            return mapper.Map<PublicUrlResponse>(url);
        }

        public async Task<UrlResponse> DeactivateUrl(string id, string userId)
        {
            var url = await repo.GetAsync(u => u.Id == id && u.UserId == userId);
            if (url is null) throw new NotFoundException("No Url Found");
            url.Deactivate();
            repo.Update(url);
            await repo.SaveAsync();
            return mapper.Map<UrlResponse>(url);
        }

        public async Task<UrlResponse> ActivateUrl(string id, string userId)
        {
            var url = await repo.GetAsync(u => u.Id == id && u.UserId == userId);
            if (url is null) throw new NotFoundException("No Url Found");
            url.Activate();
            repo.Update(url);
            await repo.SaveAsync();
            return mapper.Map<UrlResponse>(url);
        }
    }
}