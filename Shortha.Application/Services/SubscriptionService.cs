using Shortha.Application.Exceptions;
using Shortha.Domain.Entites;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Application.Services;

public interface ISubscriptionService
{
    // Define methods for subscription management
    Task<Subscription> Subscribe(string userId, string planId);
    Task Unsubscribe(string userId);
    Task<bool> IsSubscribed(string userId);
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

 

    public async Task<bool> IsSubscribed(string userId)
    {
        
        var subscription = await repo.GetAsync(s => s.UserId == userId && s.IsActive);
        return subscription != null;
        
    }

    public async Task Unsubscribe(string userId)
    {
        var subscription = await repo.GetAsync(s => s.UserId == userId && s.IsActive);
        if (subscription == null)
        {
            throw new NotFoundException($"Subscription for user {userId} not found.");
        }

        subscription.Deactivate();
        repo.Update(subscription);
        await repo.SaveAsync();
       
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