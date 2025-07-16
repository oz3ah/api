using System.Linq.Expressions;
using Shortha.Domain.Entites;

namespace Shortha.Domain.Interfaces.Repositories;

public interface IPaymentRepository
{
    Task<Payment> CreatePayment(Payment payment);

    Task<ICollection<Payment>?> GetPaymentsByUserId(string userId,
                                                    Expression<Func<Payment, bool>>? whereExpression = null);

    Task<Payment?> GetPaymentById(string paymentId);
    Task<Payment?> GetPaymentByTransactionId(string transactionId);
    Task<Payment> Update(Payment payment);
}