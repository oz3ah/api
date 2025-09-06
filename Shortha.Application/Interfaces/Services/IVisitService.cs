using Shortha.Application.Dto.Responses.Visit;
using Shortha.Domain;
using Shortha.Domain.Dto;

namespace Shortha.Application.Interfaces.Services
{
    public interface IVisitService
    {
        Task Record(RequestInfo request, string urlId);

        //Task<PaginationResult<Visit>> GetVisitsByUser(string userId, int page = 1, int pageSize = 10);
        Task<PaginationResult<VisitsResponse>> GetVisitsByShortUrl(string shortUrl, int page = 1, int pageSize = 1);
    }
}