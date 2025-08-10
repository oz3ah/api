using Microsoft.Extensions.Logging;
using Shortha.Application.Dto.Responses.Url;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Application.Services;

public class AnalyticsService(ILogger<AnalyticsService> logger, IUrlRepository repo) : IAnalyticsService
{
    public async Task<UserUrlStatsResponse> GetUserStats(string userId)
    {
        var totalUrlsCount = await repo.CountAsync(u => u.UserId == userId);
        var totalClicksCount = await repo.GetTotalClicksByUserId(userId);
        var totalActiveUrlsCount = await repo.CountAsync(u => u.UserId == userId && u.IsActive);
        var totalThisMonth =
            await repo.CountAsync(u => u.UserId == userId && u.CreatedAt >= DateTime.UtcNow.AddMonths(-1));

        logger.LogInformation("User {UserId} stats: TotalUrlsCount={TotalUrlsCount}, TotalClicksCount={TotalClicksCount}, TotalActiveUrlsCount={TotalActiveUrlsCount}, TotalThisMonth={TotalThisMonth}");

        return new UserUrlStatsResponse()
        {
            TotalActiveUrlsCount = totalActiveUrlsCount,
            TotalClicksCount = totalClicksCount,
            TotalUrlsCount = totalUrlsCount,
            TotalThisMonth = totalThisMonth
        };
    }
}

public interface IAnalyticsService
{
    Task<UserUrlStatsResponse> GetUserStats(string userId);
}