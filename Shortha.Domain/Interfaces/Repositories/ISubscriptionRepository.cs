using Shortha.Domain.Entites;

namespace Shortha.Domain.Interfaces.Repositories
{
    public interface ISubscriptionRepository
    {
        Task<Subscription> CreateSubscription(Subscription subscription);
        Task<Subscription?> GetSubscriptionByUserId(string userId);
        Task<bool> UpdateSubscription(Subscription subscription);
        Task<int> CancelByUserId(string userId);
    }
}