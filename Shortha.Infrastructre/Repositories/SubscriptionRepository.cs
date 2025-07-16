using Microsoft.EntityFrameworkCore;
using Shortha.Domain.Entites;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Infrastructre.Repositories
{
    public class SubscriptionRepository(AppDb context) : ISubscriptionRepository
    {
        public async Task<Subscription?> GetSubscriptionByUserId(string userId)
        {
            return await context.Subscriptions
                                .Where(s => s.UserId == userId).FirstOrDefaultAsync(s => s.IsActive);
        }

        public async Task<Subscription> CreateSubscription(Subscription subscription)
        {
            await context.Subscriptions.AddAsync(subscription);
            await context.SaveChangesAsync();
            return subscription;
        }

        public async Task<bool> UpdateSubscription(Subscription subscription)
        {
            context.Subscriptions.Update(subscription);
            return await context.SaveChangesAsync() > 0;
        }


        public async Task<int> CancelByUserId(string userId)
        {
            return await context.Subscriptions
                                .Where(s => s.UserId == userId && s.IsActive)
                                .ExecuteUpdateAsync(s => s.SetProperty(subscription => subscription.IsActive, false));
        }
    }
}