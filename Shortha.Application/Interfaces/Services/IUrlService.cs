using Shortha.Application.Dto.Requests.Url;
using Shortha.Application.Dto.Responses.Url;
using Shortha.Domain;
using Shortha.Domain.Dto;

namespace Shortha.Application.Interfaces.Services;

public interface IUrlService
{
    Task<UrlResponse> CreateUrl(UrlCreateRequest urlCreate, string? userId, bool isPro, string source);
    Task<PaginationResult<UrlResponse>> GetUrlsByUserId(string userId, int page = 1, int pageSize = 10);
    Task<PublicUrlResponse> OpenUrl(string shortUrl, RequestInfo request);
    Task<UrlResponse> DeactivateUrl(string id, string userId);
    Task<UrlResponse> ActivateUrl(string id, string userId);
}