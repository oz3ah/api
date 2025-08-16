using AutoMapper;
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
    Task<Subscription?> Get(string userId);
    Task Unsubscribe(string userId);
    Task<SubscriptionCreationResponse?> IsSubscribed(string userId);

    Task<Subscription> UpgradeSubscription(string paymentHash, string transactionId, string method,
        string currency);

    Task<Subscription> Update(Subscription updatedSubscription);
}

public class SubscriptionService(
    ISubscriptionRepository repo,
    IPackagesService packages,
    IPaymentService payments,
    IMapper mapper,
    IUnitOfWork ef)
    : ISubscriptionService
{
    public async Task<SubscriptionCreationResponse> Subscribe(string userId, string planId)
    {
        // Start a transaction to ensure atomicity

        await ef.BeginTransactionAsync();

        try
        {
            var Current = await Get(userId);

            // Check if has pending subscription, pending subscription means the user has not completed the payment yet
            var pendingPayment = await payments.GetPendingByUser(userId);


            if (pendingPayment is not null && Current is not null && Current.IsPending)
            {
                return mapper.Map<SubscriptionCreationResponse>(Current);
            }

            var package = await packages.GetPackageDetails(planId);
            if (package.Name == PackagesName.Free)
            {
                throw new NoPermissionException("Free package does not require a subscription.");
            }

            if (pendingPayment is null && Current is not null && Current is not { IsActive: true })
            {
                // Regeratate Payment Link
                var updatedPayment = await payments.Create(package, userId);
                Current.PaymentId = updatedPayment.Id;
                await Update(Current);
                await ef.CommitAsync();
                return mapper.Map<SubscriptionCreationResponse>(Current);
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

            // Commit the transaction
            await ef.CommitAsync();
            return mapper.Map<SubscriptionCreationResponse>(subscription);
        }
        catch (Exception e)
        {
            await ef.RollbackAsync();
            throw;
        }
    }

    public async Task<Subscription?> Get(string userId)
    {
        return await repo.GetAsync(s => s.UserId == userId, includes: new[]
        {
            "Payment", "Package"
        });
    }


    public async Task<SubscriptionCreationResponse?> IsSubscribed(string userId)
    {
        var subscription = await repo.GetAsync(
            s => s.UserId == userId && s.IsActive,
            includes: ["Payment", "Package"]);

        return subscription is null ? null : mapper.Map<SubscriptionCreationResponse>(subscription);
    }


    public async Task Unsubscribe(string userId)
    {
        var subscription = await repo.GetAsync(s => s.UserId == userId && s.IsActive);
        if (subscription is not null)
        {
            subscription.Deactivate();
            repo.Update(subscription);
            await repo.SaveAsync();
        }
    }

    public async Task<Subscription> UpgradeSubscription(string paymentHash, string transactionId, string method,
        string currency)
    {
        await ef.BeginTransactionAsync();


        try
        {
            var payment = await payments.GetPaymentByHash(paymentHash);
            payment.TransactionId = transactionId;
            payment.PaymentMethod = method;
            payment.Currency = currency;
            payment.Status = PaymentStatus.Completed;
            payment.PaymentDate = DateTime.UtcNow;

            payment = await payments.Update(payment);

            await Unsubscribe(payment.UserId);

            var sub = await repo.GetAsync(s => s.PaymentId == payment.Id, includes: ["Package"]);
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

    public async Task<Subscription> Update(Subscription updatedSubscription)
    {
        var existingSubscription = await repo.GetByIdAsync(updatedSubscription.Id);
        if (existingSubscription == null)
        {
            throw new NotFoundException($"Subscription with ID {updatedSubscription.Id} does not exist.");
        }

        repo.Update(updatedSubscription);
        await repo.SaveAsync();
        return updatedSubscription;
    }
}