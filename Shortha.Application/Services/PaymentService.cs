using System.Security.Cryptography;
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
    private string GeneratePaymentHash(string userId, string packageId, decimal price)
    {
        var prime1 = Crypto.GetRandomPrime();
        var prime2 = Crypto.GetRandomPrime();
        long primeProduct = prime1 * prime2;

        var raw = $"{userId}.{packageId}.{price}.{primeProduct}.gitnasr";
        return Crypto.GenerateSHA265FromRaw(raw);
    }

    public async Task<Payment> Create(Package package, string userId)
    {
        var hash = GeneratePaymentHash(userId, package.Id, package.Price);


        var paymentLink = kahsier.Url(hash, package);
        var payment = new Payment
        {
            UserId = userId,
            Amount = package.Price,
            PackageId = package.Id,
            PaymentLink = paymentLink,
            PaymentHash = hash
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