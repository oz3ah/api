using Shortha.Domain.Entites;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Infrastructre.Repositories
{
    public class PaymentRepository(AppDb context) : GenericRepository<Payment>(context), IPaymentRepository
    {
    }
}