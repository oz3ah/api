using Shortha.Domain.Entites;
using Shortha.Domain.Interfaces.Repositories;

namespace Shortha.Infrastructre.Repositories
{
    public class SubscriptionRepository(AppDb context)
        : GenericRepository<Subscription>(context), ISubscriptionRepository
    {
    }
}