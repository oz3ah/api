using Shortha.Application.Exceptions;
using Shortha.Domain.Entites;
using Shortha.Domain.Enums;
using Shortha.Domain.Interfaces;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Application.Services;

public interface ISubscriptionService
{
    Task<Subscription> Subscribe(string userId, string planId);
    Task Unsubscribe(string userId);
    Task<bool> IsSubscribed(string userId);

    Task<Subscription> UpgradeSubscription(string paymentId, string transactionId, string method,
                                           string currency);
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

    public async Task<Subscription> UpgradeSubscription(string paymentId, string transactionId, string method,
                                                        string currency)
    {
        await ef.BeginTransactionAsync();

        try
        {
            var transactionUpdate = new PaymentUpdateDto()
            {
                TransactionId = transactionId,
                PaymentMethod = method,
                Currency = currency,
            };

            var payment = await payments.Update(transactionUpdate, paymentId);
            if (payment == null)
            {
                throw new UpdateFailedException("Failed to update payment.");
            }

            await Unsubscribe(payment.UserId);
            var newSubscription = new Subscription
            {
                UserId = payment.UserId,
                PackageId = payment.PackageId,
                PaymentId = payment.Id,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(payment.Package.DurationInDays),
            };

            await repo.AddAsync(newSubscription);

            await repo.SaveAsync();
            await ef.CommitAsync();

            return newSubscription;
        }
        catch
        {
            await ef.RollbackAsync();
            throw;
        }
    }
}