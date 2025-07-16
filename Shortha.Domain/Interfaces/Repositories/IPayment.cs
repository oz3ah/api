using Shortha.Domain.Entites;

namespace Shortha.Domain.Interfaces.Repositories;

public interface IPayment
{
    string GetPaymentLink(string orderId, Package package);
}