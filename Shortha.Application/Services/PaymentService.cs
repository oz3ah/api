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
}

public interface IPaymentService
{
    // Define methods for payment processing, e.g., ProcessPayment, RefundPayment, etc.
    Task<Payment> Create(Package package, string userId);
    Task<Payment> CreateVoid(string packageId, string userId);
    Task<Payment?> GetPendingByUser(string userId);
    string GeneratePaymentLink(string subscriptionId, Package package);
    Task<Payment> Update(PaymentUpdateDto paymentUpdateDto, string paymentId);
}

public class PaymentService(IPaymentRepository repo, IKahsierService kahsier) : IPaymentService
{
    public async Task<Payment> Create(Package package, string userId)
    {
        var payment = new Payment
                      {
                          UserId = userId,
                          Amount = package.Price,
                          PackageId = package.Id,
                      };
        await repo.AddAsync(payment);
        await repo.SaveAsync();
        return payment;
    }

    public string GeneratePaymentLink(string subscriptionId, Package package)
    {
        var paymentLink = kahsier.Url(subscriptionId, package);

        return paymentLink;
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
        return payment;
    }

    public async Task<Payment> Update(PaymentUpdateDto paymentUpdateDto, string paymentId)
    {
        var payment = await repo.GetAsync(p => p.Id == paymentId);
        if (payment == null)
        {
            throw new NotFoundException("Payment not found");
        }

        if (paymentUpdateDto.Status.HasValue)
        {
            payment.Status = paymentUpdateDto.Status.Value;
        }

        if (!string.IsNullOrEmpty(paymentUpdateDto.Currency))
        {
            payment.Currency = paymentUpdateDto.Currency;
        }

        if (!string.IsNullOrEmpty(paymentUpdateDto.TransactionId))
        {
            payment.TransactionId = paymentUpdateDto.TransactionId;
        }

        if (!string.IsNullOrEmpty(paymentUpdateDto.PaymentMethod))
        {
            payment.PaymentMethod = paymentUpdateDto.PaymentMethod;
        }

        if (!string.IsNullOrEmpty(paymentUpdateDto.PaymentLink))
        {
            payment.PaymentLink = paymentUpdateDto.PaymentLink;
        }

        repo.Update(payment);
        await repo.SaveAsync();
        return payment;
    }
}