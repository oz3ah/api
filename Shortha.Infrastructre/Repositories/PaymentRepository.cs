using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Shortha.Domain.Entites;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Infrastructre.Repositories
{
    public class PaymentRepository(AppDb context) : IPaymentRepository
    {
        public async Task<Payment> CreatePayment(Payment payment)
        {
            await context.Payments.AddAsync(payment);
            await context.SaveChangesAsync();
            return payment;
        }

        public async Task<ICollection<Payment>?> GetPaymentsByUserId(string userId,
                                                                     Expression<Func<Payment, bool>>? whereExpression =
                                                                         null)
        {
            var payments = context.Payments.AsQueryable();
            payments = payments.Where(p => p.UserId == userId);
            if (whereExpression != null)
            {
                payments = payments.Where(whereExpression);
            }

            return await payments.ToListAsync();
        }

        public async Task<Payment?> GetPaymentById(string paymentId)
        {
            return await context.Payments.Where(p => p.Id == paymentId)
                                .FirstOrDefaultAsync();
        }


        public async Task<Payment?> GetPaymentByTransactionId(string transactionId)
        {
            return await context.Payments.Where(p => p.TransactionId == transactionId)
                                .FirstOrDefaultAsync();
        }


        public async Task<Payment> Update(Payment entity)
        {
            context.Payments.Update(entity);
            var result = await context.SaveChangesAsync();
            return entity;
        }
    }
}