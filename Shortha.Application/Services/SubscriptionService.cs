using Shortha.Domain.Entites;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Application.Services;

public interface ISubscriptionService
{
    // Define methods for subscription management
    Task<Subscription> Subscribe(string userId, string planId);
    void Unsubscribe(string userId);
    bool IsSubscribed(string userId);
    void UpgradeSubscription(string userId, string newPlanId);
    void DowngradeSubscription(string userId, string newPlanId);
}

public class SubscriptionService(ISubscriptionRepository repo) : ISubscriptionService
{
    public async Task<Subscription> Subscribe(string userId, string planId)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(planId))
        {
            throw new ArgumentException("User ID and Plan ID cannot be null or empty.");
        }

        var subscription = new Subscription
        {
            UserId = userId,
            PackageId = planId,
            PaymentId = null
        };
        await repo.AddAsync(subscription);
        await repo.SaveAsync();
        return subscription;
    }

    public void Unsubscribe(string userId)
    {
        throw new NotImplementedException();
    }

    public bool IsSubscribed(string userId)
    {
        throw new NotImplementedException();
    }

    public void UpgradeSubscription(string userId, string newPlanId)
    {
        throw new NotImplementedException();
    }

    public void DowngradeSubscription(string userId, string newPlanId)
    {
        throw new NotImplementedException();
    }
}