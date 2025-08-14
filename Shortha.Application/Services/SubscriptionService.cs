using Shortha.Application.Dto.Responses.Subscription;
using Shortha.Application.Exceptions;
using Shortha.Domain.Entites;
using Shortha.Domain.Enums;
using Shortha.Domain.Interfaces;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Application.Services;

public interface ISubscriptionService
{
    Task<SubscriptionCreationResponse> Subscribe(string userId, string planId);
    Task Unsubscribe(string userId);
    Task<SubscriptionCreationResponse?> IsSubscribed(string userId);

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
    public async Task<SubscriptionCreationResponse> Subscribe(string userId, string planId)
    {
        // Start a transaction to ensure atomicity

        await ef.BeginTransactionAsync();

        try
        {
            var Current = await IsSubscribed(userId);
            // Check if the user is already subscribed
            if (Current != null)
            {
                return Current;
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
            var paymentLink = payments.GeneratePaymentLink(payment.Id, package);

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


            return new SubscriptionCreationResponse()
            {
                Id = subscription.Id,
                StartDate = subscription.StartDate,
                CreatedAt = subscription.CreatedAt,
                UpdatedAt = subscription.UpdatedAt,
                Name = package.Name,
                Price = package.Price,
                PaymentLink = paymentLink,
                Status = payment.Status
            };
        }
        catch (Exception e)
        {
            await ef.RollbackAsync();
            throw;
        }
    }


    public async Task<SubscriptionCreationResponse?> IsSubscribed(string userId)
    {
        var subscription = await repo.GetAsync(s => s.UserId == userId && s.IsActive, includes: new[]
                                                   {
                                                       "Payment", "Package"
                                                   });
        if (subscription != null)
        {
            return new SubscriptionCreationResponse
            {
                Id = subscription.Id,
                StartDate = subscription.StartDate,
                CreatedAt = subscription.CreatedAt,
                UpdatedAt = subscription.UpdatedAt,
                Name = subscription.Package.Name,
                Price = subscription.Package.Price,
                PaymentLink = subscription.Payment.PaymentLink,
                Status = subscription.Payment.Status
            };
        }

        return null;
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
                Status = PaymentStatus.Completed,
                PaymentDate = DateTime.UtcNow
            };

            var payment = await payments.Update(transactionUpdate, paymentId);
            if (payment == null)
            {
                throw new UpdateFailedException("Failed to update payment.");
            }

            await Unsubscribe(payment.UserId);

            var sub = await repo.GetAsync(s => s.PaymentId == paymentId, includes: ["Package"]);
            if (sub == null)
            {
                throw new NotFoundException("Finding Subscription with the Payment ID Provided is not found");
            }


            sub.PackageId = payment.PackageId;
            sub.PaymentId = payment.Id;
            sub.StartDate = DateTime.UtcNow;
            sub.EndDate = DateTime.UtcNow.AddDays(sub.Package.DurationInDays);
            sub.Activate();

            repo.Update(sub);

            await repo.SaveAsync();
            await ef.CommitAsync();

            return sub;
        }
        catch
        {
            await ef.RollbackAsync();
            throw;
        }
    }
}