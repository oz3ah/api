using Shortha.Application.Exceptions;
using Shortha.Domain.Entites;
using Shortha.Domain.Enums;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Application.Services;

public class PaymentUpdateDto
{
    public PaymentStatus? Status { get; set; }
    public string? Currency { get; set; }
    public string? TransactionId { get; set; }
    public string? PaymentMethod { get; set; }

    public string? PaymentLink { get; set; }
    public DateTime? PaymentDate { get; set; }
}

public interface IPaymentService
{
    // Define methods for payment processing, e.g., ProcessPayment, RefundPayment, etc.
    Task<Payment> Create(Package package, string userId);
    Task<Payment> CreateVoid(string packageId, string userId);
    Task<Payment?> GetPendingByUser(string userId);
    Task<Payment> Update(Payment updatedPayment);
    Task<Payment> GetPaymentByHash(string paymentHash);
}

public class PaymentService(IPaymentRepository repo, IKahsierService kahsier) : IPaymentService
{
    public async Task<Payment> Create(Package package, string userId)
    {
        var paymentHash = $"{userId}_{package.Price * 3}_{DateTime.UtcNow.Nanosecond}_gitnasr_payment_system";
        var paymentLink = kahsier.Url(paymentHash, package);
        var payment = new Payment
        {
            UserId = userId,
            Amount = package.Price,
            PackageId = package.Id,
            PaymentLink = paymentLink,
            PaymentHash = paymentHash
        };
        await repo.AddAsync(payment);
        await repo.SaveAsync();
        return payment;
    }

    public async Task<Payment> GetPaymentByHash(string paymentHash)
    {
        var payment = await repo.GetAsync(p => p.PaymentHash == paymentHash);
        if (payment == null)
        {
            throw new NotFoundException("Payment not found");
        }

        return payment!;
    }

    public async Task<Payment> CreateVoid(string packageId, string userId)
    {
        var payment = new Payment
        {
            Amount = 0,
            Currency = "VOD",
            PackageId = packageId,
            PaymentMethod = "AUTO",
            TransactionId = "-1",
            Status = PaymentStatus.Completed,
            PaymentDate = DateTime.UtcNow,
            ExpirationDate = DateTime.UtcNow,
            UserId = userId
        };

        await repo.AddAsync(payment);
        await repo.SaveAsync();
        return payment;
    }

    public async Task<Payment?> GetPendingByUser(string userId)
    {
        var payment = await repo.GetAsync(u => u.UserId == userId && u.Status == PaymentStatus.Pending);
        return payment is { IsExpired: false } ? payment : null;
    }

    public async Task<Payment> Update(Payment updatedPayment)
    {
        repo.Update(updatedPayment);
        await repo.SaveAsync();
        return updatedPayment;
    }
}