using Microsoft.Extensions.Logging;
using Shortha.Application.Dto.Responses.Url;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Application.Services;

public class AnalyticsService(ILogger<AnalyticsService> logger, IUrlRepository repo) : IAnalyticsService
{
    public async Task<UserUrlStatsResponse> GetUserStats(string userId)
    {
        logger.LogInformation("Starting GetUserStats for UserId: {UserId}", userId);

        try
        {
            var totalUrlsCount = await repo.CountAsync(u => u.UserId == userId);
            logger.LogDebug("Retrieved total URLs count: {TotalUrlsCount} for UserId: {UserId}",
                totalUrlsCount, userId);

            var totalClicksCount = await repo.GetTotalClicksByUserId(userId);
            logger.LogDebug("Retrieved total clicks count: {TotalClicksCount} for UserId: {UserId}",
                totalClicksCount, userId);

            var totalActiveUrlsCount = await repo.CountAsync(u => u.UserId == userId && u.IsActive);
            logger.LogDebug("Retrieved active URLs count: {TotalActiveUrlsCount} for UserId: {UserId}",
                totalActiveUrlsCount, userId);

            var totalThisMonth = await repo.CountAsync(u => u.UserId == userId && u.CreatedAt >= DateTime.UtcNow.AddMonths(-1));
            logger.LogDebug("Retrieved this month's URLs count: {TotalThisMonth} for UserId: {UserId}",
                totalThisMonth, userId);

            var response = new UserUrlStatsResponse()
            {
                TotalActiveUrlsCount = totalActiveUrlsCount,
                TotalClicksCount = totalClicksCount,
                TotalUrlsCount = totalUrlsCount,
                TotalThisMonth = totalThisMonth
            };

            logger.LogInformation("Successfully retrieved user stats for UserId: {UserId} | " +
                "TotalUrls: {TotalUrlsCount} | TotalClicks: {TotalClicksCount} | " +
                "ActiveUrls: {TotalActiveUrlsCount} | ThisMonth: {TotalThisMonth}",
                userId, totalUrlsCount, totalClicksCount, totalActiveUrlsCount, totalThisMonth);

            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving user stats for UserId: {UserId}", userId);
            throw;
        }
    }
}

public interface IAnalyticsService
{
    Task<UserUrlStatsResponse> GetUserStats(string userId);
}