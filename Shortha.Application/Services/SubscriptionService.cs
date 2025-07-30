using Shortha.Application.Exceptions;
using Shortha.Domain.Entites;
using Shortha.Domain.Enums;
using Shortha.Domain.Interfaces;
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

public class SubscriptionService(
    ISubscriptionRepository repo,
    IPackagesService packages,
    IPaymentService payments,
    IUnitOfWork ef)
    : ISubscriptionService
{
    public async Task<Subscription> Subscribe(string userId, string planId)
    {
        // Start a transaction to ensure atomicity

        await ef.BeginTransactionAsync();

        try
        {
            // Check if the user is already subscribed
            if (await IsSubscribed(userId))
            {
                throw new ConflictException($"User {userId} is already subscribed.");
            }

            var package = await packages.GetPackageDetails(planId);
            if (package.Name == PackagesName.Free)
            {
                throw new NoPermissionException("Free package does not require a subscription.");
            }

            var payment = await payments.Create(package, userId);

            var subscription = new Subscription
            {
                UserId = userId,
                PackageId = planId,
                PaymentId = payment.Id,
            };
            await repo.AddAsync(subscription);
            await repo.SaveAsync();
            // Create payment link
            var paymentLink = payments.GeneratePaymentLink(subscription.Id, package);

            // Update the payment with the payment link

            var payload = new PaymentUpdateDto
            {
                PaymentLink = paymentLink
            };

            await payments.Update(payload, payment.Id);
            await repo.SaveAsync();

            // Commit the transaction
            await ef.CommitAsync();
            // Return the created subscription


            return subscription;
        }
        catch (Exception e)
        {
            await ef.RollbackAsync();
            throw;
        }
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