using AutoMapper;
using IPinfo;
using Shortha.Application.Dto.Responses.Visit;
using Shortha.Domain;
using Shortha.Domain.Dto;
using Shortha.Domain.Entites;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Application.Services
{

    public class VisitService(IVisitRepository repo, IMapper mapper, IPinfoClient client) : IVisitService
    {
        private async Task<Tracker> DispatchTracker(RequestInfo request)
        {
            var tracker = new TrackerBuilder(request.userAgent)
                                .WithBrowser()
                                .WithOs()
                                .WithBrand()
                                .WithModel()
                                .WithIp(request.ipAddress)
                                .WithDevice()
                                .Build();

            tracker.UserId = request.userId;
            var ipInfo = await client.IPApi.GetDetailsAsync(request.ipAddress);
            tracker.Country = ipInfo.Country;
            tracker.City = ipInfo.City;
            tracker.TimeZone = ipInfo.Timezone;
            return tracker;
        }
        public async Task Record(RequestInfo request, string urlId)
        {
            var tracker = await DispatchTracker(request);
            var visit = new Visit
            {
                Browser = tracker.BrowserName,
                DeviceBrand = tracker.Device,
                DeviceType = tracker.Model,
                IpAddress = tracker.IpAddress,
                City = tracker.City,
                IsBot = tracker.IsBot,
                Os = tracker.OsName,
                Region = tracker.Region,
                UserAgent = tracker.UserAgent,
                Country = tracker.Country,
                TimeZone = tracker.TimeZone,
                UrlId = urlId,
            };
            await repo.AddAsync(visit);
            await repo.SaveAsync();

        }

        //public async Task<PaginationResult<Visit>> GetVisitsByUser(string userId, int page = 1, int pageSize = 10)
        //{
        //    var visits = await repo.GetAsync(u => u.Url.UserId == userId, page, pageSize, "Url");
        //    return new PaginationResult<VisitsResponse>
        //    {
        //        Items = visits.Items,
        //        TotalCount = visits.TotalCount,
        //        PageNumber = page,
        //        PageSize = pageSize,

        //    };
        //}

        public async Task<PaginationResult<VisitsResponse>> GetVisitsByShortUrl(string shortUrl, int page = 1, int pageSize = 1)
        {


            var visits = await repo.GetAsync(u => u.Url.ShortCode == shortUrl, page, pageSize);

            return mapper.Map<PaginationResult<VisitsResponse>>(visits);
        }
    }
}
